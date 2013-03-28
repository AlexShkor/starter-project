using Microsoft.Practices.ServiceLocation;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Options;
using DQF.Common.Interceptors;
using DQF.Databases;
using DQF.Platform.Dispatching;
using DQF.Platform.Dispatching.Interfaces;
using DQF.Platform.Domain;
using DQF.Platform.Domain.EventBus;
using DQF.Platform.Domain.Interfaces;
using DQF.Platform.Domain.Transitions.Interfaces;
using DQF.Platform.Domain.Transitions.Mongo;
using DQF.Platform.Mongo;
using DQF.Platform.Settings;
using DQF.Platform.StructureMap;
using DQF.Views;
using StructureMap;
using Uniform;
using Uniform.Mongodb;

namespace DQF
{
    public class Bootstaper
    {
        public void Configure(IContainer container)
        {
            ConfigureSettings(container);
            ConfigureMessageBus(container);
            ConfigureMongoDb(container);
            ConfigureTransport(container);
            ConfigureEventStore(container);
            ConfigureUniform(container);
            ServiceLocator.SetLocatorProvider(() => new StructureMapServiceLocator(container));
        }

        public void ConfigureSettings(IContainer container)
        {
            var settings = SettingsMapper.Map<SiteSettings>();

            container.Configure(config => config.For<SiteSettings>().Singleton().Use(settings));

        }

        public void ConfigureMessageBus(IContainer container)
        {
            container.Configure(config => config.For<ICommandBus>().Use<CommandBus>());
        }

        public void ConfigureMongoDb(IContainer container)
        {
            // Register bson serializer conventions
            var myConventions = new ConventionProfile();
            myConventions.SetIdMemberConvention(new NoDefaultPropertyIdConvention());
            myConventions.SetIgnoreExtraElementsConvention(new AlwaysIgnoreExtraElementsConvention());
            BsonClassMap.RegisterConventions(myConventions, type => true);
            DateTimeSerializationOptions.Defaults = DateTimeSerializationOptions.LocalInstance;

            RegisterBsonMaps();

            var settings = container.GetInstance<SiteSettings>();
            container.Configure(config =>
            {
                config.For<MongoViewDatabase>().Singleton().Use(new MongoViewDatabase(settings.MongoViewConnectionString).EnsureIndexes());
                config.For<MongoLogsDatabase>().Singleton().Use(new MongoLogsDatabase(settings.MongoLogsConnectionString).EnsureIndexes());
                config.For<MongoEventsDatabase>().Singleton().Use(new MongoEventsDatabase(settings.MongoEventsConnectionString));
            });
        }

        private static void RegisterBsonMaps()
        {
            //BsonClassMap.RegisterClassMap<UserView>();
        }

        public void ConfigureEventStore(IContainer container)
        {
            var settings = container.GetInstance<SiteSettings>();
            var dispatcher = container.GetInstance<IDispatcher>();

            var transitionsRepository = new MongoTransitionRepository(settings.MongoEventsConnectionString);

            container.Configure(config =>
            {
                config.For<ITransitionRepository>().Singleton().Use(transitionsRepository);
                config.For<IEventBus>().Singleton().Use(new DispatcherEventBus(dispatcher));
                config.For<IRepository>().Use<Repository>();
                config.For(typeof(IRepository<>)).Use(typeof(Repository<>));
            });
        }

        public void ConfigureUniform(IContainer container)
        {
            var settings = container.GetInstance<SiteSettings>();

            // 1. Create databases
            var mongodbDatabase = new MongodbDatabase(settings.MongoViewConnectionString);

            // 2. Configure uniform 
            var uniform = UniformDatabase.Create(config => config
                .RegisterDocuments(typeof(UserView).Assembly)
                .RegisterDatabase(ViewDatabases.Mongodb, mongodbDatabase));

            container.Configure(config => config.For<UniformDatabase>().Singleton().Use(uniform));
        }

        private void ConfigureTransport(IContainer container)
        {
            var serviceLocator = new StructureMapServiceLocator(container);
            var dispatcher = Dispatcher.Create(d => d
                .AddHandlers(typeof(UserView).Assembly, type => container.Configure(c => c.For(typeof(IMessageHandler)).HybridHttpOrThreadLocalScoped().Use(type)))
                .AddInterceptor(typeof(LoggingInterceptor))
                .SetServiceLocator(serviceLocator));

            container.Configure(config =>
            {
                //config.Scan(scan =>
                //{
                //    scan.AssemblyContainingType<UserView>();
                //    scan.AddAllTypesOf<IMyMessageHandler>();
                //});
                config.For<IHandlersAgregator>().HybridHttpOrThreadLocalScoped().Use<HandlersAgregator>();
                config.For<ICommandBus>().Use<CommandBus>();
                config.For<IDispatcher>().Singleton().Use(dispatcher);
                config.For<IServiceLocator>().Singleton().Use(serviceLocator);
            });
        }
    }
}
