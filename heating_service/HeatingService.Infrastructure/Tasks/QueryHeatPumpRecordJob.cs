using HeatingService.Application.Services.HeatPump;
using HeatingService.Application.Tasks;
using HeatingService.Domain.HeatPump;
using HeatingService.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HeatingService.Infrastructure.Tasks;

public class QueryHeatPumpRecordJob : IQueryHeatPumpRecordJob {
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
    try {
      var heatPumpService = scope.ServiceProvider.GetRequiredService<HeatPumpService>();
      var heatPumpRecordRepository = scope.ServiceProvider.GetRequiredService<HeatPumpRecordRepository>();
      
      var tankLimits = await heatPumpService.GetTankLimitsAsync();
      if (tankLimits.IsError) {
        throw new Exception(tankLimits.FirstError.Description);
      }

      var temperatures = await heatPumpService.GetTemperaturesAsync();
      if (temperatures.IsError) {
        throw new Exception(temperatures.FirstError.Description);
      }

      var now = DateTime.UtcNow;
      var timeStamp = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0, DateTimeKind.Utc);
      var heatPumpRecord = new HeatPumpRecord()
      {
        TankLimits = tankLimits.Value,
        Temperatures = temperatures.Value,
        TimeStamp = timeStamp
      };

      heatPumpRecordRepository.Add(heatPumpRecord);
      await heatPumpRecordRepository.SaveAsync();
    
      _logger.LogInformation("Queried HeatPumpRecord at: " + timeStamp + "Z");
    }
    catch (Exception e)
    {
      _logger.LogError(e.Message);
    }
  }
}
