namespace HeatingGateway.Contracts.Heating; 

public record GetCompressorRecordsRequest(
  DateTime From,
  DateTime To);
