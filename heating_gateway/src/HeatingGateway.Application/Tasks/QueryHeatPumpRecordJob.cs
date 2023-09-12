using CronScheduler.Extensions.Scheduler;
using HeatingGateway.Application.Domain;
using HeatingGateway.Application.Persistence.Repositories;
using HeatingGateway.Application.Services.HeatPump;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ErrorOr;
using HeatingGateway.Application.Common.Errors;

namespace HeatingGateway.Application.Tasks;

public class QueryHeatPumpRecordJob : IScheduledJob {
  private readonly QueryHeatPumpRecordJobOptions _options;
  private readonly IServiceProvider _provider;
  private readonly ILogger<QueryHeatPumpRecordJob> _logger;

  public QueryHeatPumpRecordJob(
    IOptionsMonitor<QueryHeatPumpRecordJobOptions> options,
    IServiceProvider provider,
    ILogger<QueryHeatPumpRecordJob> logger) {
    _options = options.Get(Name);
    _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
  }

  public string Name { get; } = nameof(QueryHeatPumpRecordJob);

  private static DateTime RoundToNearestMinute(DateTime dt) {
    var d = TimeSpan.FromMinutes(1);
    var delta = dt.Ticks % d.Ticks;
    bool roundUp = delta > d.Ticks / 2;
    var offset = roundUp
      ? d.Ticks
      : 0;

    return new DateTime(dt.Ticks + offset - delta, dt.Kind);
  }

  private async Task<ErrorOr<Compressor>> QueryCompressor() {
    using var scope = _provider.CreateScope();
    try {
      var heatPumpRecordRepository = scope.ServiceProvider.GetRequiredService<HeatPumpRecordRepository>();
      var heatPumpService = scope.ServiceProvider.GetRequiredService<HeatPumpService>();

      var compressorActive = await heatPumpService.IsCompressorActiveAsync();
      if (compressorActive.IsError) {
        throw new Exception(compressorActive.FirstError.Description);
      }

      var now = DateTime.UtcNow;
      var time = RoundToNearestMinute(now);
      _logger.LogCritical("PARSING TIME: {time}", time);

      var previous = await heatPumpRecordRepository.FindLatestBeforeAsync(time);
      if (previous is null)
        return new Compressor(compressorActive.Value, null);

      if (previous.Compressor.Active == compressorActive.Value)
        return Errors.Compressor.UnableToComputeUsage;

      var b = await heatPumpRecordRepository.FindLatestBeforeAsync(time, !compressorActive.Value);
      if (b is null)
        return new Compressor(compressorActive.Value, null);

      var a = await heatPumpRecordRepository.FindLatestBeforeAsync(b.Time, compressorActive.Value);
      if (a is null)
        return new Compressor(compressorActive.Value, null);

      var ab = (b.Time - a.Time).TotalMinutes;
      var ac = (time - a.Time).TotalMinutes;
      return new Compressor(compressorActive.Value,
        compressorActive.Value
          ? ab / ac
          : 1.0 - ab / ac);
    } catch (Exception e) {
      _logger.LogError(e.Message);
      return Error.Failure(e.Message);
    }
  }

  public async Task ExecuteAsync(CancellationToken cancellationToken) {
    using var scope = _provider.CreateScope();
    try {
      var heatPumpService = scope.ServiceProvider.GetRequiredService<HeatPumpService>();
      var heatPumpRecordRepository = scope.ServiceProvider.GetRequiredService<HeatPumpRecordRepository>();

      var compressor = await QueryCompressor();
      if (compressor.IsError && compressor.FirstError != Errors.Compressor.UnableToComputeUsage) {
        throw new Exception(compressor.FirstError.Description);
      }

      var tankLimits = await heatPumpService.GetTankLimitsAsync();
      if (tankLimits.IsError) {
        throw new Exception(tankLimits.FirstError.Description);
      }

      var temperatures = await heatPumpService.GetTemperaturesAsync();
      if (temperatures.IsError) {
        throw new Exception(temperatures.FirstError.Description);
      }

      var now = DateTime.UtcNow;
      var time = RoundToNearestMinute(now);
      var heatPumpRecord = new HeatPumpRecord() {
        Compressor = compressor.Value,
        TankLimits = tankLimits.Value,
        Temperatures = temperatures.Value,
        Time = time
      };

      heatPumpRecordRepository.Add(heatPumpRecord);
      await heatPumpRecordRepository.SaveAsync();

      _logger.LogInformation("Queried HeatPumpRecord at: " + time + "Z");
    } catch (Exception e) {
      _logger.LogError(e.Message);
    }
  }
}
