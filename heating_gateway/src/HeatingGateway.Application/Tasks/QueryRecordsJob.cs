using CronScheduler.Extensions.Scheduler;
using ErrorOr;
using HeatingGateway.Application.Common.Errors;
using HeatingGateway.Application.Domain;
using HeatingGateway.Application.Persistence.Repositories;
using HeatingGateway.Application.Services.HeatPump;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HeatingGateway.Application.Tasks;

public class QueryRecordsJob : IScheduledJob {
  private readonly ILogger<QueryRecordsJob> _logger;
  private readonly QueryRecordsJobOptions _options;
  private readonly IServiceProvider _provider;

  public QueryRecordsJob(
    IOptionsMonitor<QueryRecordsJobOptions> options,
    IServiceProvider provider,
    ILogger<QueryRecordsJob> logger) {
    _options = options.Get(Name);
    _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
  }

  public string Name { get; } = nameof(QueryRecordsJob);

  public async Task ExecuteAsync(CancellationToken cancellationToken) {
    using var scope = _provider.CreateScope();
    try {
      var compressorRecordRepository = scope.ServiceProvider.GetRequiredService<CompressorRecordRepository>();
      var tankLimitRecordRepository = scope.ServiceProvider.GetRequiredService<TankLimitRecordRepository>();
      var temperatureRecordRepository = scope.ServiceProvider.GetRequiredService<TemperatureRecordRepository>();

      var compressor = await QueryCompressor();
      if (compressor.IsError && compressor.FirstError != Errors.Compressor.UnableToComputeUsage) {
        throw new Exception(compressor.FirstError.Description);
      }

      var tankLimits = await QueryTankLimits();
      if (tankLimits.IsError) {
        throw new Exception(tankLimits.FirstError.Description);
      }

      var temperatures = await QueryTemperatures();
      if (temperatures.IsError) {
        throw new Exception(temperatures.FirstError.Description);
      }

      var now = DateTime.UtcNow;
      var time = RoundToNearestMinute(now);

      var compressorRecord = new CompressorRecord(
        time,
        compressor.Value.Active,
        compressor.Value.Usage);


      var tankLimitRecord = new TankLimitRecord(
        time,
        tankLimits.Value.LowerTankMinimum,
        tankLimits.Value.LowerTankMinimumAdjusted,
        tankLimits.Value.LowerTankMaximum,
        tankLimits.Value.LowerTankMaximumAdjusted,
        tankLimits.Value.UpperTankMinimum,
        tankLimits.Value.UpperTankMinimumAdjusted,
        tankLimits.Value.UpperTankMaximum,
        tankLimits.Value.UpperTankMaximumAdjusted);


      var temperatureRecord = new TemperatureRecord(
        time,
        temperatures.Value.Circuit1,
        temperatures.Value.Circuit2,
        temperatures.Value.Circuit3,
        temperatures.Value.GroundInput,
        temperatures.Value.GroundOutput,
        temperatures.Value.HotGas,
        temperatures.Value.Inside,
        temperatures.Value.LowerTank,
        temperatures.Value.Outside,
        temperatures.Value.UpperTank);

      // TODO Transaction
      compressorRecordRepository.Add(compressorRecord);
      tankLimitRecordRepository.Add(tankLimitRecord);
      temperatureRecordRepository.Add(temperatureRecord);
      await compressorRecordRepository.SaveAsync();
      await tankLimitRecordRepository.SaveAsync();
      await temperatureRecordRepository.SaveAsync();
    } catch (Exception e) {
      _logger.LogError(e.Message);
    }
  }

  private static DateTime RoundToNearestMinute(DateTime dt) {
    var d = TimeSpan.FromMinutes(1);
    var delta = dt.Ticks % d.Ticks;
    var roundUp = delta > d.Ticks / 2;
    var offset = roundUp
      ? d.Ticks
      : 0;

    return new DateTime(dt.Ticks + offset - delta, dt.Kind);
  }

  private async Task<ErrorOr<Compressor>> QueryCompressor() {
    using var scope = _provider.CreateScope();
    try {
      var compressorRecordRepository = scope.ServiceProvider.GetRequiredService<CompressorRecordRepository>();
      var heatPumpService = scope.ServiceProvider.GetRequiredService<HeatPumpService>();

      var compressorActive = await heatPumpService.IsCompressorActiveAsync();
      if (compressorActive.IsError) {
        throw new Exception(compressorActive.FirstError.Description);
      }

      var now = DateTime.UtcNow;
      var time = RoundToNearestMinute(now);

      var previous = await compressorRecordRepository.FindLatestBeforeAsync(time);
      if (previous is null) {
        return new Compressor(compressorActive.Value, null);
      }

      if (previous.Active == compressorActive.Value) {
        return new Compressor(compressorActive.Value, null);
      }

      // At this point, the compressor has gone 0 -> 1 or 1 -> 0, i.e. the (approximate) state has changed
      // Compute usage as follows:
      // Let C be the current moment, let B be the previous state change (0 -> 1 or 1 -> 0) and, let A be the state change before B.
      // e.g.       A           B       C
      //      0 0 0 1 1 1 1 1 1 0 0 0 0 1
      // We shall compute the proportion p = [A, B] / [A, C].
      // Then, the usage is either p or (1.0 - p) depending on the current state C. 

      var b = await compressorRecordRepository.FindLatestBeforeAsync(time, !compressorActive.Value, true);
      if (b is null) {
        return new Compressor(compressorActive.Value, compressorActive.Value ? 0.0 : 1.0);
      }

      var a = await compressorRecordRepository.FindLatestBeforeAsync(b.Time, compressorActive.Value, true);
      if (a is null) {
        return new Compressor(compressorActive.Value, compressorActive.Value ? 1.0 : 0.0);
      }

      var ab = (b.Time - a.Time).TotalMinutes;
      var ac = (time - a.Time).TotalMinutes;

      return new Compressor(compressorActive.Value,
        compressorActive.Value
          ? ab / ac
          : 1.0 - (ab / ac));
    } catch (Exception e) {
      _logger.LogError(e.Message);
      return Error.Failure(e.Message);
    }
  }

  private async Task<ErrorOr<TankLimits>> QueryTankLimits() {
    using var scope = _provider.CreateScope();
    try {
      var heatPumpService = scope.ServiceProvider.GetRequiredService<HeatPumpService>();
      var tankLimits = await heatPumpService.GetTankLimitsAsync();
      return tankLimits;
    } catch (Exception e) {
      _logger.LogError(e.Message);
      return Error.Failure(e.Message);
    }
  }

  private async Task<ErrorOr<Temperatures>> QueryTemperatures() {
    using var scope = _provider.CreateScope();
    try {
      var heatPumpService = scope.ServiceProvider.GetRequiredService<HeatPumpService>();
      var temperatures = await heatPumpService.GetTemperaturesAsync();
      return temperatures;
    } catch (Exception e) {
      _logger.LogError(e.Message);
      return Error.Failure(e.Message);
    }
  }
}
