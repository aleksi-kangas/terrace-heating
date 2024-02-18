using CronScheduler.Extensions.Scheduler;
using HeatingGateway.Application.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HeatingGateway.Application.Tasks;

public class RemoveOldRecordsJob : IScheduledJob {
  private readonly RemoveOldRecordsJobOptions _options;
  private readonly IServiceProvider _provider;
  private readonly ILogger<RemoveOldRecordsJob> _logger;

  public RemoveOldRecordsJob(
    IOptionsMonitor<RemoveOldRecordsJobOptions> options,
    IServiceProvider provider,
    ILogger<RemoveOldRecordsJob> logger) {
    _options = options.Get(Name);
    _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
  }

  public string Name { get; } = nameof(RemoveOldRecordsJob);

  public async Task ExecuteAsync(CancellationToken cancellationToken) {
    using var scope = _provider.CreateScope();
    try {
      var compressorRecordRepository = scope.ServiceProvider.GetRequiredService<CompressorRecordRepository>();
      var tankLimitRecordRepository = scope.ServiceProvider.GetRequiredService<TankLimitRecordRepository>();
      var temperatureRecordRepository = scope.ServiceProvider.GetRequiredService<TemperatureRecordRepository>();
      var now = DateTime.UtcNow;
      var twoWeeksBefore = now.Subtract(TimeSpan.FromDays(14));
      await compressorRecordRepository.DeleteOlderThanAsync(twoWeeksBefore);
      await tankLimitRecordRepository.DeleteOlderThanAsync(twoWeeksBefore);
      await temperatureRecordRepository.DeleteOlderThanAsync(twoWeeksBefore);
    } catch (Exception e) {
      _logger.LogError(e.Message);
    }
  }
}
