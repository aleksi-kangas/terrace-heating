namespace HeatingGateway.Contracts.Heating; 

public record CompressorRecordResponse(
  bool Active,
  DateTime Time,
  double? Usage);
