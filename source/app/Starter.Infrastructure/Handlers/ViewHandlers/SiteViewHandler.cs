﻿using DQF.Databases;
using DQF.Domain.Aggregates.Site.Events;
using DQF.Platform.Dispatching;
using DQF.Platform.Dispatching.Interfaces;
using DQF.Views;
using Uniform;

namespace DQF.Handlers.ViewHandlers
{
    public class SiteViewHandler : IMessageHandler
    {
        private readonly IDocumentCollection<SiteView> _sites;

        public SiteViewHandler(ViewDatabase db)
        {
            _sites = db.Sites;
        }

        public void Handle(SiteCreated e)
        {
            _sites.Save(e.Id, site => { });
        }

        public void Handle(SiteSettingsUpdated e)
        {
            _sites.Update(e.Id, site => site.SmtpSettings = e.SmtpSettings);
        }

        public void Handle(SchedulerStarted e)
        {
            _sites.Update(e.Id, site => site.SchedulerStopped = false);
        }

        public void Handle(SchedulerStopped e)
        {
            _sites.Update(e.Id, site =>
            {
                site.SchedulerStopped = true;
                site.SchedulerRestartNeeded = e.Restart;
            });
        }
    }
}