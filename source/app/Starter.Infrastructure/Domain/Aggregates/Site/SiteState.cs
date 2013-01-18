using DQF.Domain.Aggregates.Site.Events;

namespace DQF.Domain.Aggregates.Site
{
    public class SiteState
    {
        public string Id { get; set; }

        public void On(SiteCreated e)
        {
            Id = e.Id;
        }
    }
}