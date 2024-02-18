namespace HeatingGateway.Contracts.History; 

public record HeatPumpRecordResponse(
  CompressorResponse Compressor,
  TankLimitsResponse TankLimits,
  TemperaturesResponse Temperatures,
  DateTime Time);
