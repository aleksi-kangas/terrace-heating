using HeatingService.Contracts.HeatPump;

namespace HeatingService.Contracts.Heating; 

public record HeatPumpRecordResponse(
  TankLimitsResponse TankLimits,
  TemperaturesResponse Temperatures,
  DateTime TimeStamp);
