using CronScheduler.Extensions.Scheduler;

namespace HeatingService.Infrastructure.Tasks; 

public class QueryHeatPumpRecordJobOptions : SchedulerOptions {
  public QueryHeatPumpRecordJobOptions() {
    CronSchedule = "* * * * *";
    RunImmediately = false;
  }
}
