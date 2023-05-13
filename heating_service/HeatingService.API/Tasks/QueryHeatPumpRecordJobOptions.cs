using CronScheduler.Extensions.Scheduler;

namespace HeatingService.API.Tasks; 

public class QueryHeatPumpRecordJobOptions : SchedulerOptions {
  public QueryHeatPumpRecordJobOptions() {
    CronSchedule = "* * * * *";
    RunImmediately = false;
  }
}
