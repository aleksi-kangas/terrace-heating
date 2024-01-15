using ErrorOr;
using BoostingSchedule = HeatingGateway.Application.Domain.BoostingSchedule;
using TankLimits = HeatingGateway.Application.Domain.TankLimits;
using Temperatures = HeatingGateway.Application.Domain.Temperatures;

namespace HeatingGateway.Application.Services.HeatPump;

public interface IHeatPumpService : IDisposable {
  /*
   * Get the number of active circuits.
   */
  Task<ErrorOr<UInt32>> GetActiveCircuitCountAsync();
  
  /*
   * Get the boosting schedule for circuit 3.
   */
  Task<ErrorOr<BoostingSchedule>> GetCircuit3BoostingScheduleAsync();
  
  /*
   * Get the boosting schedule for the lower tank.
   */
  Task<ErrorOr<BoostingSchedule>> GetLowerTankBoostingScheduleAsync();
  
  /*
   * Get the temperatures.
   */
  Task<ErrorOr<Temperatures>> GetTemperaturesAsync();
  
  /*
   * Get the tank limits.
   */
  Task<ErrorOr<TankLimits>> GetTankLimitsAsync();
  
  /*
   * Check whether the compressor is active.
   */
  Task<ErrorOr<Boolean>> IsCompressorActiveAsync();
  
  /*
   * Check whether the scheduled boosting is enabled.
   */
  Task<ErrorOr<Boolean>> IsSchedulingEnabledAsync();
  
  /*
   * Set the number of active circuits.
   */
  Task<ErrorOr<Success>> SetActiveCircuitCountAsync(UInt32 activeCircuitCount);
  
  /*
   * Set the boosting schedule for circuit 3.
   */
  Task<ErrorOr<Success>> SetCircuit3BoostingScheduleAsync(BoostingSchedule boostingSchedule);
  
  /*
   * Set the boosting schedule for the lower tank.
   */
  Task<ErrorOr<Success>> SetLowerTankBoostingScheduleAsync(BoostingSchedule boostingSchedule);
  
  /*
   * Set the state of scheduled boosting.
   */
  Task<ErrorOr<Success>> SetSchedulingEnabledAsync(bool enabled);
}
