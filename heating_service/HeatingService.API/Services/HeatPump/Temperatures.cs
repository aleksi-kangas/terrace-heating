namespace HeatingService.API.Services.HeatPump; 

public record Temperatures(
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
