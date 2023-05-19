using HeatingService.Application.Domain;
using HeatingService.Contracts.Heating;
using HeatingService.Contracts.HeatPump;
using Mapster;

namespace HeatingService.API.Common.Mapping; 

public class HeatPumpMappingConfig : IRegister {
  public void Register(TypeAdapterConfig config) {
    config.NewConfig<BoostingSchedule, BoostingScheduleResponse>();
    config.NewConfig<WeekdayBoostingSchedule, WeekdayBoostingScheduleResponse>();
    config.NewConfig<TankLimits, TankLimitsResponse>();
    config.NewConfig<Temperatures, TemperaturesResponse>();
    config.NewConfig<HeatPump.BoostingSchedule, BoostingSchedule>();
    config.NewConfig<HeatPump.WeekdayBoostingSchedule, WeekdayBoostingSchedule>();
    config.NewConfig<HeatPump.TankLimits, TankLimits>();
    config.NewConfig<HeatPump.Temperatures, Temperatures>();
    config.NewConfig<HeatPumpRecord, HeatPumpRecordResponse>();
  }
}
