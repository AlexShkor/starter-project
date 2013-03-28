using DQF.Platform.Domain.Messages;

namespace DQF.Domain.Aggregates.Site.Commands
{
    public class StopScheduler : SiteCommand
    {
        public bool Restart { get; set; }
    }
}