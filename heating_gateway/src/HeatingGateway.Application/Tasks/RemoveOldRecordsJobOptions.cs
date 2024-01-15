using CronScheduler.Extensions.Scheduler;

namespace HeatingGateway.Application.Tasks; 

public class RemoveOldRecordsJobOptions : SchedulerOptions {
  public RemoveOldRecordsJobOptions() {
    CronSchedule = "0 0 * * *";
    RunImmediately = false;
  }
}
