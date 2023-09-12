using HeatingGateway.Application.Domain;
using Mapster;

namespace HeatingGateway.Application.Common.Mapping;

public class HeatPumpMappingConfig : IRegister {
  public void Register(TypeAdapterConfig config) {
    config.NewConfig<HeatPumpProto.BoostingSchedule, BoostingSchedule>();
    config.NewConfig<HeatPumpProto.WeekdayBoostingSchedule, WeekdayBoostingSchedule>();
    config.NewConfig<HeatPumpProto.TankLimits, TankLimits>();
    config.NewConfig<HeatPumpProto.Temperatures, Temperatures>();

    config.NewConfig<BoostingSchedule, HeatPumpProto.BoostingSchedule>();
    config.NewConfig<WeekdayBoostingSchedule, HeatPumpProto.WeekdayBoostingSchedule>();
  }
}
