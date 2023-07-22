using HeatingGateway.Application.Domain;
using HeatingGateway.Contracts.Heating;
using HeatingGateway.Contracts.HeatPump;
using Mapster;

namespace HeatingGateway.API.Common.Mapping;

public class HeatingMappingConfig : IRegister {
  public void Register(TypeAdapterConfig config) {
    config.NewConfig<CompressorRecord, CompressorRecordResponse>();
    config.NewConfig<HeatPumpRecord, HeatPumpRecordResponse>();
  }
}
