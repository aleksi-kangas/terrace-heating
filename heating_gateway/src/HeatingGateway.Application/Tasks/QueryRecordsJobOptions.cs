using CronScheduler.Extensions.Scheduler;

namespace HeatingGateway.Application.Tasks; 

public class QueryRecordsJobOptions : SchedulerOptions {
  public QueryRecordsJobOptions() {
    CronSchedule = "* * * * *";
    RunImmediately = false;
  }
}
