using HeatingService.Application.Domain;
using Mapster;

namespace HeatingService.Application.Common.Mapping;

public class HeatPumpMappingConfig : IRegister {
  public void Register(TypeAdapterConfig config) {
    config.NewConfig<HeatPump.BoostingSchedule, BoostingSchedule>();
    config.NewConfig<HeatPump.WeekdayBoostingSchedule, WeekdayBoostingSchedule>();
    config.NewConfig<HeatPump.TankLimits, TankLimits>();
    config.NewConfig<HeatPump.Temperatures, Temperatures>();

    config.NewConfig<BoostingSchedule, HeatPump.BoostingSchedule>();
    config.NewConfig<WeekdayBoostingSchedule, HeatPump.WeekdayBoostingSchedule>();
  }
}
