namespace HeatingGateway.Contracts.History; 

public record GetCompressorRecordsRequest(
  DateTime From,
  DateTime To);
