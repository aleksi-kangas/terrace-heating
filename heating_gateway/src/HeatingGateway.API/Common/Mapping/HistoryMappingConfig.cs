using HeatingGateway.Application.Domain;
using HeatingGateway.Contracts.History;
using Mapster;

namespace HeatingGateway.API.Common.Mapping;

public class HistoryMappingConfig : IRegister {
  public void Register(TypeAdapterConfig config) {
    config.NewConfig<CompressorRecord, CompressorRecordResponse>();
    config.NewConfig<TankLimitRecord, TankLimitRecordResponse>();
    config.NewConfig<TemperatureRecord, TemperatureRecordResponse>();
  }
}
