using CronScheduler.Extensions.Scheduler;
using HeatingGateway.Application.Domain;
using HeatingGateway.Application.Persistence.Repositories;
using HeatingGateway.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ErrorOr;
using HeatingGateway.Application.Common.Errors;

namespace HeatingGateway.Application.Tasks;

public class ComputeHeatingStateJob : IScheduledJob {
  private readonly ComputeHeatingStateJobOptions _options;
  private readonly IServiceProvider _provider;
  private readonly ILogger<ComputeHeatingStateJob> _logger;

  public ComputeHeatingStateJob(
    IOptionsMonitor<ComputeHeatingStateJobOptions> options,
    IServiceProvider provider,
    ILogger<ComputeHeatingStateJob> logger) {
    _options = options.Get(Name);
    _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
  }

  public string Name { get; } = nameof(ComputeHeatingStateJob);

  public async Task ExecuteAsync(CancellationToken cancellationToken) {
    using var scope = _provider.CreateScope();
    try {
      var heatingStateService = scope.ServiceProvider.GetRequiredService<IHeatingStateService>();
      var heatingState = await heatingStateService.ComputeHeatingStateAsync();
      if (heatingState.IsError) {
        _logger.LogError(heatingState.FirstError.Description);
      } else {
        _logger.LogInformation($"Heating state: {heatingState.Value} at {DateTime.UtcNow}");
      }
    } catch (Exception e) {
      _logger.LogError(e.Message);
    }
  }
}
