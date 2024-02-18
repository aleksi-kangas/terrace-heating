namespace HeatingGateway.Contracts.History;

public record TemperaturesResponse(
  float Circuit1,
  float Circuit2,
  float Circuit3,
  float GroundInput,
  float GroundOutput,
  float HotGas,
  float Inside,
  float LowerTank,
  float Outside,
  float UpperTank);
