using ErrorOr;
using HeatingGateway.Application.Common.Errors;
using HeatingGateway.Application.Domain;
using HeatingGateway.Application.Services.HeatPump;
using Microsoft.Extensions.DependencyInjection;

namespace HeatingGateway.Application.Services.Heating;

public class HeatingStateService : IHeatingStateService, IDisposable {
  private static readonly Mutex _mutex = new Mutex(false);
  private readonly IServiceScope _serviceScope;
  private HeatingState _heatingState = HeatingState.Inactive;
  private Timer? _softStartTimer = null;

  public HeatingStateService(IServiceProvider serviceProvider) {
    _serviceScope = serviceProvider.CreateScope();
  }

  public HeatingState GetHeatingState() {
    try {
      _mutex.WaitOne();
      return _heatingState;
    } finally {
      _mutex.ReleaseMutex();
    }
  }

  public ErrorOr<HeatingState> ComputeHeatingState() {
    try {
      _mutex.WaitOne();
      using var heatPumpService =
        _serviceScope.ServiceProvider.GetRequiredService<IHeatPumpService>();
      var activeCircuitCount = heatPumpService.GetActiveCircuitCountAsync();
      activeCircuitCount.Wait();
      if (activeCircuitCount.Result.IsError)
        return activeCircuitCount.Result.FirstError;

      var isCircuit3Active = activeCircuitCount.Result.Value == 3;
      if (!isCircuit3Active) {
        _heatingState = HeatingState.Inactive;
        return _heatingState;
      }

      if (_softStartTimer is not null) {
        _heatingState = HeatingState.SoftStarting;
        return _heatingState;
      }

      var isSchedulingEnabled = heatPumpService.IsSchedulingEnabledAsync();
      if (isSchedulingEnabled.Result.IsError)
        return isSchedulingEnabled.Result.FirstError;
      if (!isSchedulingEnabled.Result.Value) {
        _heatingState = HeatingState.Active;
        return _heatingState;
      }

      var circuit3BoostingSchedule = heatPumpService.GetCircuit3BoostingScheduleAsync();
      if (circuit3BoostingSchedule.Result.IsError)
        return circuit3BoostingSchedule.Result.FirstError;

      var now = DateTime.UtcNow;
      var isBoosting = IsTimeWithinBoostingSchedule(now, circuit3BoostingSchedule.Result.Value);
      _heatingState = isBoosting
        ? HeatingState.Boosting
        : HeatingState.Active;
      return _heatingState;
    } finally {
      _mutex.ReleaseMutex();
    }
  }

  private static bool IsTimeWithinBoostingSchedule(DateTime dateTime,
    BoostingSchedule boostingSchedule) {
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

  public ErrorOr<HeatingState> Start(bool softStart) {
    var state = ComputeHeatingState();
    if (state.IsError)
      return state.FirstError;
    if (state.Value is HeatingState.Active or HeatingState.Boosting or HeatingState.SoftStarting)
      return Errors.HeatingState.AlreadyStarted;
    _mutex.WaitOne();
    using var heatPumpService =
      _serviceScope.ServiceProvider.GetRequiredService<IHeatPumpService>();
    // 1) Ensure scheduling is initially disabled.
    var schedulingDisableResult = heatPumpService.SetSchedulingEnabledAsync(false);
    schedulingDisableResult.Wait();
    if (schedulingDisableResult.Result.IsError)
      return schedulingDisableResult.Result.FirstError;
    // 2) Enable heat distribution circuit 3.
    var circuit3EnableResult = heatPumpService.SetActiveCircuitCountAsync(3);
    circuit3EnableResult.Wait();
    if (circuit3EnableResult.Result.IsError)
      return circuit3EnableResult.Result.FirstError;
    if (softStart) {
      // 3) Enable scheduling after 12 hours.
      _softStartTimer = new Timer(SoftStartEnableScheduling, null, TimeSpan.FromHours(12),
        Timeout.InfiniteTimeSpan);
    } else {
      // 3) Enable scheduling immediately.
      var schedulingEnableResult = heatPumpService.SetSchedulingEnabledAsync(true);
      schedulingEnableResult.Wait();
      if (schedulingEnableResult.Result.IsError)
        return schedulingEnableResult.Result.FirstError;
    }

    _mutex.ReleaseMutex();
    return ComputeHeatingState();
  }

  public ErrorOr<HeatingState> Stop() {
    var state = ComputeHeatingState();
    if (state.IsError)
      return state.FirstError;
    if (state.Value is HeatingState.Inactive)
      return Errors.HeatingState.AlreadyStopped;
    _mutex.WaitOne();
    using var heatPumpService =
      _serviceScope.ServiceProvider.GetRequiredService<IHeatPumpService>();
    // 1) Disable scheduling.
    var schedulingDisableResult = heatPumpService.SetSchedulingEnabledAsync(false);
    schedulingDisableResult.Wait();
    if (schedulingDisableResult.Result.IsError)
      return schedulingDisableResult.Result.FirstError;
    // 2) Disable heat distribution circuit 3.
    var circuit3DisableResult = heatPumpService.SetActiveCircuitCountAsync(2);
    circuit3DisableResult.Wait();
    if (circuit3DisableResult.Result.IsError)
      return circuit3DisableResult.Result.FirstError;
    _mutex.ReleaseMutex();
    return ComputeHeatingState();
  }

  public void Dispose() {
    _serviceScope?.Dispose();
  }

  private void SoftStartEnableScheduling(object? _) {
    try {
      _mutex.WaitOne();
      using var heatPumpService =
        _serviceScope.ServiceProvider.GetRequiredService<IHeatPumpService>();
      var schedulingEnableResult = heatPumpService.SetSchedulingEnabledAsync(true);
      schedulingEnableResult.Wait();
    } finally {
      _softStartTimer?.Dispose();
      _softStartTimer = null;
      _mutex.ReleaseMutex();
    }
  }
}
