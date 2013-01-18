using DQF.Platform.Domain.Messages;

namespace DQF.Domain.Aggregates.Site.Events
{
    public class SchedulerStopped: Event
    {
        public bool Restart { get; set; }
    }
}