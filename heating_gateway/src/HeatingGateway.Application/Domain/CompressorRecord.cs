namespace HeatingGateway.Application.Domain;

public record CompressorRecord(
  DateTime Time,
  bool Active,
  double? Usage);
