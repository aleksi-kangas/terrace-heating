using HeatingGateway.Contracts.HeatPump;

namespace HeatingGateway.Contracts.Heating; 

public record HeatPumpRecordResponse(
  CompressorResponse Compressor,
  TankLimitsResponse TankLimits,
  TemperaturesResponse Temperatures,
  DateTime Time);
