namespace HeatingService.API.Services.HeatPump; 

public record TankLimits(
  UInt32 LowerTankMinimum,
  UInt32 LowerTankMaximum,
  UInt32 UpperTankMinimum,
  UInt32 UpperTankMaximum);
