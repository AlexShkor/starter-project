using DQF.Platform.Domain.Messages;

namespace DQF.Domain.Aggregates.Site.Commands
{
    public class StopScheduler:Command
    {
        public bool Restart { get; set; }
    }
}