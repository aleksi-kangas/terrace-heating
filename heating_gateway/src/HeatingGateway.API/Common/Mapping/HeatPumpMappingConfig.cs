using HeatingGateway.Application.Domain;
using HeatingGateway.Contracts.HeatPump;
using Mapster;

namespace HeatingGateway.API.Common.Mapping;

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
