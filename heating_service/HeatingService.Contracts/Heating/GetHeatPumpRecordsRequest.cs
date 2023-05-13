namespace HeatingService.Contracts.Heating; 

public record GetHeatPumpRecordsRequest(
  DateTime From,
  DateTime To);