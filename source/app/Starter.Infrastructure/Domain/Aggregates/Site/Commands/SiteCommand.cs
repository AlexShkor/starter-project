using DQF.Platform.Domain.Messages;

namespace DQF.Domain.Aggregates.Site.Commands
{
    public abstract class SiteCommand: Command
    {
        public string SiteId { get; set; }
    }
}