namespace HeatingService.Contracts.HeatPump;

public record TankLimitsResponse(
  UInt32 LowerTankMinimum,
  UInt32 LowerTankMaximum,
  UInt32 UpperTankMinimum,
  UInt32 UpperTankMaximum);
