using CronScheduler.Extensions.Scheduler;
using HeatingGateway.Application.Domain;
using HeatingGateway.Application.Persistence.Repositories;
using HeatingGateway.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HeatingGateway.Application.Tasks;

public class ComputeCompressorUsageJob : IScheduledJob {
  private readonly ComputeCompressorUsageJobOptions _options;
  private readonly IServiceProvider _provider;
  private readonly ILogger<ComputeCompressorUsageJob> _logger;

  public ComputeCompressorUsageJob(
    IOptionsMonitor<ComputeCompressorUsageJobOptions> options,
    IServiceProvider provider,
    ILogger<ComputeCompressorUsageJob> logger) {
    _options = options.Get(Name);
    _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
  }

  public string Name { get; } = nameof(ComputeCompressorUsageJob);

  public async Task ExecuteAsync(CancellationToken cancellationToken) {
    using var scope = _provider.CreateScope();
    try {
      var heatPumpService = scope.ServiceProvider.GetRequiredService<HeatPumpService>();
      var compressorRecordRepository =
        scope.ServiceProvider.GetRequiredService<CompressorRecordRepository>();

      var compressorActive = await heatPumpService.IsCompressorActiveAsync();
      if (compressorActive.IsError) {
        throw new Exception(compressorActive.FirstError.Description);
      }
      var now = DateTime.UtcNow;
      var time = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0, DateTimeKind.Utc);
      var c = new CompressorRecord() { Active = compressorActive.Value, Time = time };

      var previous = await compressorRecordRepository.FindLatestBeforeAsync(c.Time);
      if (previous is null) {
        c.Usage = -1.0;
        compressorRecordRepository.Add(c);
        await compressorRecordRepository.SaveAsync();
        return;
      }

      if (previous.Active == c.Active)
        return;

      var b = await compressorRecordRepository.FindLatestBeforeAsync(c.Time, !c.Active);
      if (b is null) {
        c.Usage = -1.0;
        compressorRecordRepository.Add(c);
        await compressorRecordRepository.SaveAsync();
        return;
      }

      var a = await compressorRecordRepository.FindLatestBeforeAsync(b.Time, c.Active);
      if (a is null) {
        c.Usage = -1.0;
        compressorRecordRepository.Add(c);
        await compressorRecordRepository.SaveAsync();
        return;
      }

      var ab = (b.Time - a.Time).TotalMinutes;
      var ac = (c.Time - a.Time).TotalMinutes;
      c.Usage = c.Active ? ab / ac : 1.0 - ab / ac;
      compressorRecordRepository.Add(c);
      await compressorRecordRepository.SaveAsync();
    }
    catch (Exception e) {
      _logger.LogError(e.Message);
    }
  }
}
