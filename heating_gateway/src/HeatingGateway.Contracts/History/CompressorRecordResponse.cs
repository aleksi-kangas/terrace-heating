namespace HeatingGateway.Contracts.History; 

public record CompressorRecordResponse(
  DateTime Time,
  bool Active,
  double? Usage);
