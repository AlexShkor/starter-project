using Microsoft.Practices.Unity;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Options;
using DQF.Databases;
using DQF.Platform.Dispatching;
using DQF.Platform.Dispatching.Interfaces;
using DQF.Platform.Domain;
using DQF.Platform.Domain.EventBus;
using DQF.Platform.Domain.Interfaces;
using DQF.Platform.Domain.Transitions.Interfaces;
using DQF.Platform.Domain.Transitions.Mongo;
using DQF.Platform.Mongo;
using DQF.Views;
using Uniform;
using Uniform.Mongodb;

namespace DQF
{
    public static class Configuration
    {
        public static void ConfigureSettings(IUnityContainer container)
        {
            container.RegisterInstance(new SiteSettings()
            {
                MongoEventsConnectionString = "mongodb://admin(admin):1@localhost:27020/dqf_events",
                MongoViewConnectionString = "mongodb://admin(admin):1@localhost:27020/dqf_view",
                MongoLogsConnectionString = "mongodb://admin(admin):1@localhost:27020/dqf_logs"
            });
        }

        public static void ConfigureMongoDB(IUnityContainer container)
        {
            var settings = container.Resolve<SiteSettings>();
            container.RegisterInstance(new MongoViewDatabase(settings.MongoViewConnectionString).EnsureIndexes());
            container.RegisterInstance(new MongoLogsDatabase(settings.MongoLogsConnectionString).EnsureIndexes());
            container.RegisterInstance(new MongoEventsDatabase(settings.MongoEventsConnectionString));

            // Register bson serializer conventions
            var myConventions = new ConventionProfile();
            myConventions.SetIdMemberConvention(new NoDefaultPropertyIdConvention());
            myConventions.SetIgnoreExtraElementsConvention(new AlwaysIgnoreExtraElementsConvention());
            BsonClassMap.RegisterConventions(myConventions, type => true);
            DateTimeSerializationOptions.Defaults = DateTimeSerializationOptions.UtcInstance;
        }

        public static void ConfigureEventStore(IUnityContainer container)
        {
            var settings = container.Resolve<SiteSettings>();
            var dispatcher = container.Resolve<IDispatcher>();

            var transitionsRepository = new MongoTransitionRepository(settings.MongoEventsConnectionString);

            container.RegisterInstance<ITransitionRepository>(transitionsRepository)
                .RegisterInstance<IEventBus>(new DispatcherEventBus(dispatcher))
                .RegisterType<IRepository, Repository>()
                .RegisterType(typeof(IRepository<>), typeof(Repository<>));
        }

        public static void ConfigureUniform(IUnityContainer container)
        {
            var settings = container.Resolve<SiteSettings>();

            // 1. Create databases
            var mongodbDatabase = new MongodbDatabase(settings.MongoViewConnectionString);

            // 2. Configure uniform 
            var uniform = UniformDatabase.Create(config => config
                .RegisterDocuments(typeof(UserView).Assembly)
                .RegisterDatabase(ViewDatabases.Mongodb, mongodbDatabase));

            container.RegisterInstance(uniform);
        }
    }
}