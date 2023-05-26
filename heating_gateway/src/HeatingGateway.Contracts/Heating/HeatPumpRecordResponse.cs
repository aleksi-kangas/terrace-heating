using HeatingGateway.Contracts.HeatPump;

namespace HeatingGateway.Contracts.Heating; 

public record HeatPumpRecordResponse(
  TankLimitsResponse TankLimits,
  TemperaturesResponse Temperatures,
  DateTime Time);
