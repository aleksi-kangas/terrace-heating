namespace HeatingGateway.Contracts.HeatPump;

public record PutWeekdayBoostingScheduleRequest(
  UInt32 StartHour,
  UInt32 EndHour,
  Int32 TemperatureDelta);

public record PutBoostingScheduleRequest(
  PutWeekdayBoostingScheduleRequest Monday,
  PutWeekdayBoostingScheduleRequest Tuesday,
  PutWeekdayBoostingScheduleRequest Wednesday,
  PutWeekdayBoostingScheduleRequest Thursday,
  PutWeekdayBoostingScheduleRequest Friday,
  PutWeekdayBoostingScheduleRequest Saturday,
  PutWeekdayBoostingScheduleRequest Sunday);
