namespace HeatingService.Domain.HeatPump; 

public record WeekdayBoostingSchedule(
  UInt32 StartHour,
  UInt32 EndHour,
  UInt32 TemperatureDelta);

public record BoostingSchedule(
  WeekdayBoostingSchedule Monday,
  WeekdayBoostingSchedule Tuesday,
  WeekdayBoostingSchedule Wednesday,
  WeekdayBoostingSchedule Thursday,
  WeekdayBoostingSchedule Friday,
  WeekdayBoostingSchedule Saturday,
  WeekdayBoostingSchedule Sunday);
