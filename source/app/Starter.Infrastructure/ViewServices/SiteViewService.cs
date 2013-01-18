using DQF.Databases;
using DQF.Platform.Mongo;
using DQF.Platform.ViewServices;
using DQF.Views;

namespace DQF.ViewServices
{
    public class SitesViewService : ViewService<SiteView>
    {
        public SitesViewService(MongoViewDatabase database)
            : base(database)
        {
        }

        protected override ReadonlyMongoCollection<SiteView> Items
        {
            get { return Database.Sites; }
        }

        public SiteView GetSite()
        {
            return GetById(SiteSettings.SiteId);
        }
    }
}