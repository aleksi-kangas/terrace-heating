using ErrorOr;
using BoostingSchedule = HeatingGateway.Application.Domain.BoostingSchedule;
using TankLimits = HeatingGateway.Application.Domain.TankLimits;
using Temperatures = HeatingGateway.Application.Domain.Temperatures;

namespace HeatingGateway.Application.Services.HeatPump;

public interface IHeatPumpService : IDisposable {
  Task<ErrorOr<UInt32>> GetActiveCircuitCountAsync();
  Task<ErrorOr<BoostingSchedule>> GetCircuit3BoostingScheduleAsync();
  Task<ErrorOr<BoostingSchedule>> GetLowerTankBoostingScheduleAsync();
  Task<ErrorOr<Temperatures>> GetTemperaturesAsync();
  Task<ErrorOr<TankLimits>> GetTankLimitsAsync();
  Task<ErrorOr<Boolean>> IsCompressorActiveAsync();
  Task<ErrorOr<Boolean>> IsSchedulingEnabledAsync();
  Task<ErrorOr<Success>> SetActiveCircuitCountAsync(UInt32 activeCircuitCount);
  Task<ErrorOr<Success>> SetCircuit3BoostingScheduleAsync(BoostingSchedule boostingSchedule);
  Task<ErrorOr<Success>> SetLowerTankBoostingScheduleAsync(BoostingSchedule boostingSchedule);
  Task<ErrorOr<Success>> SetSchedulingEnabledAsync(bool enabled);
}
