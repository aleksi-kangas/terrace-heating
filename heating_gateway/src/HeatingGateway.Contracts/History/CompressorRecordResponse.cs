namespace HeatingGateway.Contracts.History; 

public record CompressorRecordResponse(
  bool Active,
  DateTime Time,
  double? Usage);
