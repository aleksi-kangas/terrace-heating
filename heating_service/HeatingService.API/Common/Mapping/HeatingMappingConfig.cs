using HeatingService.Application.Domain;
using HeatingService.Contracts.Heating;
using HeatingService.Contracts.HeatPump;
using Mapster;

namespace HeatingService.API.Common.Mapping;

public class HeatingMappingConfig : IRegister {
  public void Register(TypeAdapterConfig config) {
    config.NewConfig<HeatPumpRecord, HeatPumpRecordResponse>();
  }
}
