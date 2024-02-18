namespace HeatingGateway.Application.Domain; 

public record TankLimitRecord(
  DateTime Time,
  Int32 LowerTankMinimum,
  Int32 LowerTankMinimumAdjusted,
  Int32 LowerTankMaximum,
  Int32 LowerTankMaximumAdjusted,
  Int32 UpperTankMinimum,
  Int32 UpperTankMinimumAdjusted,
  Int32 UpperTankMaximum,
  Int32 UpperTankMaximumAdjusted);
