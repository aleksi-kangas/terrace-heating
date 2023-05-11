using HeatingService.Contracts.HeatPump;

namespace HeatingService.API.Services;

public interface IHeatPumpService {
  Task<UInt32> GetActiveCircuitCountAsync();
  Task<TemperaturesResponse> GetTemperaturesAsync();
  Task<TankLimitsResponse> GetTankLimitsAsync();
  Task<Boolean> IsCompressorActiveAsync();
  Task<Boolean> IsSchedulingEnabledAsync();
}
