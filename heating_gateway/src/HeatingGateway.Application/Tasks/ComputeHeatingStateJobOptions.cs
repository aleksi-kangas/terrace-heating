using CronScheduler.Extensions.Scheduler;

namespace HeatingGateway.Application.Tasks; 

public class ComputeHeatingStateJobOptions : SchedulerOptions {
  public ComputeHeatingStateJobOptions() {
    CronSchedule = "30 * * * * *";
    RunImmediately = true;
  }
}
