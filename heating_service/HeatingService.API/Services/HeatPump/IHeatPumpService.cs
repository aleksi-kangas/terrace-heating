using ErrorOr;
using HeatingService.API.Domain;

namespace HeatingService.API.Services.HeatPump;

public interface IHeatPumpService {
  Task<ErrorOr<UInt32>> GetActiveCircuitCountAsync();
  Task<ErrorOr<HeatingService.API.Domain.Temperatures>> GetTemperaturesAsync();
  Task<ErrorOr<HeatingService.API.Domain.TankLimits>> GetTankLimitsAsync();
  Task<ErrorOr<Boolean>> IsCompressorActiveAsync();
  Task<ErrorOr<Boolean>> IsSchedulingEnabledAsync();
}