using HeatingService.Contracts.HeatPump;

namespace HeatingService.API.Services;

public interface IHeatPumpService {
  Task<TemperaturesResponse> GetTemperaturesAsync();
  Task<TankLimitsResponse> GetTankLimitsAsync();
}
