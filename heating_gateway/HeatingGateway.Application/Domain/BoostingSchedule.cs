namespace HeatingGateway.Application.Domain; 

public record WeekdayBoostingSchedule(
  UInt32 StartHour,
  UInt32 EndHour,
  Int32 TemperatureDelta);

public record BoostingSchedule(
  WeekdayBoostingSchedule Monday,
  WeekdayBoostingSchedule Tuesday,
  WeekdayBoostingSchedule Wednesday,
  WeekdayBoostingSchedule Thursday,
  WeekdayBoostingSchedule Friday,
  WeekdayBoostingSchedule Saturday,
  WeekdayBoostingSchedule Sunday);
