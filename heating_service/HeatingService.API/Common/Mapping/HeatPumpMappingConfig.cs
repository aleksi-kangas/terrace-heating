using HeatingService.Application.Domain;
using HeatingService.Contracts.Heating;
using HeatingService.Contracts.HeatPump;
using Mapster;

namespace HeatingService.API.Common.Mapping;

public class HeatPumpMappingConfig : IRegister {
  public void Register(TypeAdapterConfig config) {
    config.NewConfig<PutBoostingScheduleRequest, BoostingSchedule>();
    config.NewConfig<PutWeekdayBoostingScheduleRequest, WeekdayBoostingSchedule>();

    config.NewConfig<BoostingSchedule, BoostingScheduleResponse>();
    config.NewConfig<WeekdayBoostingSchedule, WeekdayBoostingScheduleResponse>();

    config.NewConfig<TankLimits, TankLimitsResponse>();

    config.NewConfig<Temperatures, TemperaturesResponse>();
  }
}
