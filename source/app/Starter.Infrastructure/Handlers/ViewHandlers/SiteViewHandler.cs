using DQF.Databases;
using DQF.Domain.Aggregates.Site.Events;
using DQF.Platform.Dispatching;
using DQF.Platform.Dispatching.Interfaces;
using DQF.Views;
using Uniform;

namespace DQF.Handlers.ViewHandlers
{
    public class SiteViewHandler : MessageHandler
    {
        private readonly IDocumentCollection<SiteView> _sites;

        public SiteViewHandler(ViewDatabase db)
        {
            _sites = db.Sites;
            Handle((SiteCreated e) => _sites.Save(e.Id, site => { }));
            Handle((SiteSettingsUpdated e) => _sites.Update(e.Id, site => site.SmtpSettings = e.SmtpSettings));
            Handle((SchedulerStarted e) => _sites.Update(e.Id, site => site.SchedulerStopped = false));
            Handle((SchedulerStopped e) => _sites.Update(e.Id, site =>
            {
                site.SchedulerStopped = true;
                site.SchedulerRestartNeeded = e.Restart;
            }));
        }
    }
}