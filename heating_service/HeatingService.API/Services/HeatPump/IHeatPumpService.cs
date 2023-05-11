using ErrorOr;

namespace HeatingService.API.Services.HeatPump;

public interface IHeatPumpService {
  Task<ErrorOr<UInt32>> GetActiveCircuitCountAsync();
  Task<ErrorOr<Temperatures>> GetTemperaturesAsync();
  Task<ErrorOr<TankLimits>> GetTankLimitsAsync();
  Task<ErrorOr<Boolean>> IsCompressorActiveAsync();
  Task<ErrorOr<Boolean>> IsSchedulingEnabledAsync();
}