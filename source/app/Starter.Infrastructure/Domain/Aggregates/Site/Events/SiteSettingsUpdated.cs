using DQF.Domain.Aggregates.Site.Data;
using DQF.Platform.Domain;
using DQF.Platform.Domain.Messages;

namespace DQF.Domain.Aggregates.Site.Events
{
    public class SiteSettingsUpdated : Event
    {
        public SmtpSettingsData SmtpSettings { get; set; }
    }
}