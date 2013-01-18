using DQF.Domain.Aggregates.User.Events;
using Microsoft.Practices.Unity;
using DQF.Platform.Dispatching;
using DQF.Platform.Dispatching.Interfaces;
using DQF.Platform.Domain;
using DQF.Platform.Unity;
using UnityServiceLocator = Microsoft.Practices.Unity.UnityServiceLocator;

namespace DQF.Replay
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var container = AppDomainUnityContext.Current;

            // Order of configurators is important
            Configuration.ConfigureSettings(container);
            Configuration.ConfigureMongoDB(container);
            ConfigureTransport(container);
            Configuration.ConfigureEventStore(container);
            Configuration.ConfigureUniform(container);

            var replayer = container.Resolve<Replayer>();
            replayer.Start();
        }

        private static void ConfigureTransport(IUnityContainer container)
        {
            var dispatcher = Dispatcher.Create(d => d
                // Only View and Index handlers are used when replaying
                .AddHandlers(typeof(UserCreated).Assembly,
                    new[] { "DQF.Handlers.ViewHandlers", "DQF.Handlers.IndexHandlers" })
                .SetServiceLocator(new UnityServiceLocator(container)));

            container.RegisterType<ICommandBus, CommandBus>();
            container.RegisterInstance<IDispatcher>(dispatcher);
        }
    }
}
