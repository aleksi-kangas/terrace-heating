using ErrorOr;
using HeatingGateway.Application.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace HeatingGateway.Application.Services;

public class HeatingStateService : IHeatingStateService, IDisposable {
  private HeatingState _heatingState = HeatingState.Inactive;
  private readonly IServiceScope _serviceScope;

  public HeatingStateService(IServiceProvider serviceProvider) {
    _serviceScope = serviceProvider.CreateScope();
  }

  public HeatingState GetHeatingState() {
    return _heatingState;
  }

  public async Task<ErrorOr<HeatingState>> ComputeHeatingStateAsync() {
    using var heatPumpService = _serviceScope.ServiceProvider.GetRequiredService<IHeatPumpService>();
    var activeCircuitCount = await heatPumpService.GetActiveCircuitCountAsync();
    if (activeCircuitCount.IsError)
      return activeCircuitCount.FirstError;

    var isCircuit3Active = activeCircuitCount.Value == 3;
    if (!isCircuit3Active) {
      _heatingState = HeatingState.Inactive;
      return _heatingState;
    }
    
    // TODO Soft-Starting?

    var isSchedulingEnabled = await heatPumpService.IsSchedulingEnabledAsync();
    if (isSchedulingEnabled.IsError)
      return isSchedulingEnabled.FirstError;
    if (!isSchedulingEnabled.Value) {
      _heatingState = HeatingState.Active;
      return _heatingState;
    }

    var circuit3BoostingSchedule = await heatPumpService.GetCircuit3BoostingScheduleAsync();
    if (circuit3BoostingSchedule.IsError)
      return circuit3BoostingSchedule.FirstError;

    var now = DateTime.UtcNow;
    var isBoosting = IsTimeWithinBoostingSchedule(now, circuit3BoostingSchedule.Value);
    _heatingState = isBoosting
      ? HeatingState.Boosting
      : HeatingState.Active;
    return _heatingState;
  }

  private static bool IsTimeWithinBoostingSchedule(DateTime dateTime, BoostingSchedule boostingSchedule) {
    var weekdayBoostingSchedule = dateTime.DayOfWeek switch {
      DayOfWeek.Monday => boostingSchedule.Monday,
      DayOfWeek.Tuesday => boostingSchedule.Tuesday,
      DayOfWeek.Wednesday => boostingSchedule.Wednesday,
      DayOfWeek.Thursday => boostingSchedule.Thursday,
      DayOfWeek.Friday => boostingSchedule.Friday,
      DayOfWeek.Saturday => boostingSchedule.Saturday,
      DayOfWeek.Sunday => boostingSchedule.Sunday,
      _ => throw new ArgumentOutOfRangeException()
    };
    if (weekdayBoostingSchedule.StartHour == weekdayBoostingSchedule.EndHour) {
      return true; // Indicates 24 hour boosting
    }

    if (weekdayBoostingSchedule.EndHour < weekdayBoostingSchedule.StartHour) {
      return weekdayBoostingSchedule.StartHour <= dateTime.Hour ||
             dateTime.Hour <= weekdayBoostingSchedule.EndHour;
    }

    return weekdayBoostingSchedule.StartHour <= dateTime.Hour &&
           dateTime.Hour <= weekdayBoostingSchedule.EndHour;
  }

  public void Dispose() {
    _serviceScope?.Dispose();
  }
}
