using HeatingGateway.Application.Domain;
using HeatingGateway.Contracts.History;
using Mapster;

namespace HeatingGateway.API.Common.Mapping;

public class HistoryMappingConfig : IRegister {
  public void Register(TypeAdapterConfig config) {
    config.NewConfig<HeatPumpRecord, HeatPumpRecordResponse>();
    config.NewConfig<HeatPumpRecord, CompressorRecordResponse>()
      .Map(dest => dest.Active, src => src.Compressor.Active)
      .Map(dest => dest.Time, src => src.Time)
      .Map(dest => dest.Usage, src => src.Compressor.Usage);
    config.NewConfig<Compressor, CompressorResponse>();
    config.NewConfig<TankLimits, TankLimitsResponse>();
    config.NewConfig<Temperatures, TemperaturesResponse>();
  }
}
