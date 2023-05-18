namespace HeatingService.Application.Domain; 

public record TankLimits(
  UInt32 LowerTankMinimum,
  UInt32 LowerTankMaximum,
  UInt32 UpperTankMinimum,
  UInt32 UpperTankMaximum);
