using CronScheduler.Extensions.Scheduler;

namespace HeatingGateway.Application.Tasks; 

public class QueryHeatPumpRecordJobOptions : SchedulerOptions {
  public QueryHeatPumpRecordJobOptions() {
    CronSchedule = "* * * * *";
    RunImmediately = false;
  }
}
