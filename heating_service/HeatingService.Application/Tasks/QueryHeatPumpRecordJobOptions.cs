using CronScheduler.Extensions.Scheduler;

namespace HeatingService.Application.Tasks; 

public class QueryHeatPumpRecordJobOptions : SchedulerOptions {
  public QueryHeatPumpRecordJobOptions() {
    CronSchedule = "* * * * *";
    RunImmediately = false;
  }
}
