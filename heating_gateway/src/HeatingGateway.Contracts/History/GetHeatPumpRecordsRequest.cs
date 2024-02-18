namespace HeatingGateway.Contracts.History; 

public record GetHeatPumpRecordsRequest(
  DateTime From,
  DateTime To);
