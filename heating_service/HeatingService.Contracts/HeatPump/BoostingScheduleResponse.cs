namespace HeatingService.Contracts.HeatPump;

public record WeekdayBoostingScheduleResponse(
  UInt32 StartHour,
  UInt32 EndHour,
  UInt32 TemperatureDelta);

public record BoostingScheduleResponse(
  WeekdayBoostingScheduleResponse Monday,
  WeekdayBoostingScheduleResponse Tuesday,
  WeekdayBoostingScheduleResponse Wednesday,
  WeekdayBoostingScheduleResponse Thursday,
  WeekdayBoostingScheduleResponse Friday,
  WeekdayBoostingScheduleResponse Saturday,
  WeekdayBoostingScheduleResponse Sunday);