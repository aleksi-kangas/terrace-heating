namespace HeatingService.Contracts.HeatPump;

public record TankLimitsResponse(
  float LowerTankMinimum,
  float LowerTankMaximum,
  float UpperTankMinimum,
  float UpperTankMaximum);
