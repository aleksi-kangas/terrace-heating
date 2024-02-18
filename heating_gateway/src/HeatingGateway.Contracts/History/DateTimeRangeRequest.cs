namespace HeatingGateway.Contracts.History;

public record DateTimeRangeRequest(
  DateTime From,
  DateTime To);
