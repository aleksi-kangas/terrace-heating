using ErrorOr;
using BoostingSchedule = HeatingService.Application.Domain.BoostingSchedule;
using TankLimits = HeatingService.Application.Domain.TankLimits;
using Temperatures = HeatingService.Application.Domain.Temperatures;

namespace HeatingService.Application.Services.HeatPump;

public interface IHeatPumpService {
  Task<ErrorOr<UInt32>> GetActiveCircuitCountAsync();
  Task<ErrorOr<BoostingSchedule>> GetCircuit3BoostingScheduleAsync();
  Task<ErrorOr<Temperatures>> GetTemperaturesAsync();
  Task<ErrorOr<TankLimits>> GetTankLimitsAsync();
  Task<ErrorOr<Boolean>> IsCompressorActiveAsync();
  Task<ErrorOr<Boolean>> IsSchedulingEnabledAsync();
}
