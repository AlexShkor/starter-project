using Quartz;

namespace DQF.Scheduler
{
    public interface IScheduledJob : IJob
    {
        IJobDetail ConfigureJob();

        ITrigger ConfigureTrigger();
    }
}