using DQF.Domain.Aggregates.Site.Data;
using DQF.Platform.Domain;
using DQF.Platform.Domain.Messages;

namespace DQF.Domain.Aggregates.Site.Commands
{
    public class SetSiteSettings : SiteCommand
    {
        public SmtpSettingsData SmtpSettings { get; set; }
    }
}