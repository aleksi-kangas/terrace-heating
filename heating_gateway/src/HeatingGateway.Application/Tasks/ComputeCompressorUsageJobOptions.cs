using CronScheduler.Extensions.Scheduler;

namespace HeatingGateway.Application.Tasks; 

public class ComputeCompressorUsageJobOptions : SchedulerOptions {
  public ComputeCompressorUsageJobOptions() {
    CronSchedule = "* * * * *";
    RunImmediately = false;
  }
}
