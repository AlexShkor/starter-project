using DQF.Domain.Aggregates.Site.Commands;
using DQF.Platform.Dispatching;
using DQF.Platform.Dispatching.Interfaces;
using DQF.Platform.Domain;
using DQF.Platform.Domain.Interfaces;

namespace DQF.Domain.Aggregates.Site
{
    public class SiteApplicationService : MessageHandler
    {
        private readonly IRepository<SiteAggregate> _repository;

        public SiteApplicationService(IRepository<SiteAggregate> repository)
        {
            _repository = repository;
            Handle((CreateSite c) => _repository.Perform(c.SiteId, a => a.Create(c)));
            Handle((SetSiteSettings c) => _repository.Perform(c.SiteId, a => a.UpdateSettings(c)));
            Handle((StartScheduler c) => _repository.Perform(c.SiteId, a => a.StartScheduler()));
            Handle((StopScheduler c) => _repository.Perform(c.SiteId, a => a.StopScheduler(c.Restart)));
        }
    }
}