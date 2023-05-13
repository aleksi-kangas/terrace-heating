using CronScheduler.Extensions.Scheduler;
using HeatingService.API.Domain;
using HeatingService.API.Services.HeatPump;
using Microsoft.Extensions.Options;

namespace HeatingService.API.Tasks;

public class QueryHeatPumpRecordJob : IScheduledJob {
  private readonly QueryHeatPumpRecordJobOptions _options;
  private readonly IServiceProvider _provider;
  private readonly ILogger<QueryHeatPumpRecordJob> _logger;

  public QueryHeatPumpRecordJob(
    IOptionsMonitor<QueryHeatPumpRecordJobOptions> options,
    IServiceProvider provider,
    ILogger<QueryHeatPumpRecordJob> logger)
  {
    _options = options.Get(Name);
    _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
  }

  public string Name { get; } = nameof(QueryHeatPumpRecordJob);

  public async Task ExecuteAsync(CancellationToken cancellationToken)
  {
    using var scope = _provider.CreateScope();
    var heatPumpService = scope.ServiceProvider.GetRequiredService<HeatPumpService>();
    var heatingDbContext = scope.ServiceProvider.GetRequiredService<HeatingDbContext>();

    var tankLimits = await heatPumpService.GetTankLimitsAsync();
    if (tankLimits.IsError) {
      _logger.LogError(tankLimits.FirstError.Description);
      return;
    }

    var temperatures = await heatPumpService.GetTemperaturesAsync();
    if (temperatures.IsError) {
      _logger.LogError(tankLimits.FirstError.Description);
      return;
    }

    var now = DateTime.UtcNow;
    var timeStamp = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0, DateTimeKind.Utc);
    var heatPumpRecord = new HeatPumpRecord()
    {
      TankLimits = tankLimits.Value,
      Temperatures = temperatures.Value,
      TimeStamp = timeStamp
    };

    await heatingDbContext.HeatPumpRecords.AddAsync(heatPumpRecord, cancellationToken);
    await heatingDbContext.SaveChangesAsync(cancellationToken);
    
    _logger.LogInformation("Queried HeatPumpRecord at: " + timeStamp + "Z");
  }
}