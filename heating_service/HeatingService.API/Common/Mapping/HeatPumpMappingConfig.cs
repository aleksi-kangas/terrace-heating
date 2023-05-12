using HeatingService.API.Domain;
using HeatingService.Contracts.HeatPump;
using Mapster;

namespace HeatingService.API.Common.Mapping; 

public class HeatPumpMappingConfig : IRegister {
  public void Register(TypeAdapterConfig config) {
    config.NewConfig<TankLimits, TankLimitsResponse>();
    config.NewConfig<Temperatures, TemperaturesResponse>();
    config.NewConfig<HeatPump.TankLimits, TankLimits>();
    config.NewConfig<HeatPump.Temperatures, Temperatures>();
  }
}
