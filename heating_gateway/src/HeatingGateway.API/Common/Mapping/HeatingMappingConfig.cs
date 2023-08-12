using HeatingGateway.Application.Domain;
using HeatingGateway.Contracts.Heating;
using HeatingGateway.Contracts.HeatPump;
using Mapster;

namespace HeatingGateway.API.Common.Mapping;

public class HeatingMappingConfig : IRegister {
  public void Register(TypeAdapterConfig config) {
    config.NewConfig<HeatPumpRecord, HeatPumpRecordResponse>();
    config.NewConfig<HeatPumpRecord, CompressorRecordResponse>()
      .Map(dest => dest.Active, src => src.Compressor.Active)
      .Map(dest => dest.Time, src => src.Time)
      .Map(dest => dest.Usage, src => src.Compressor.Usage);
  }
}
