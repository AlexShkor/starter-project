using System;
using System.Reflection;
using Microsoft.Practices.ServiceLocation;

namespace DQF.Platform.Dispatching
{
    public static class DispatcherConfigurationExtensions
    {
        public static DispatcherConfiguration SetServiceLocator(this DispatcherConfiguration configuration, IServiceLocator container)
        {
            configuration.ServiceLocator = container;
            return configuration;
        }

        public static DispatcherConfiguration SetMaxRetries(this DispatcherConfiguration configuration, Int32 maxRetries)
        {
            configuration.NumberOfRetries = maxRetries;
            return configuration;
        }

        public static DispatcherConfiguration AddHandlers(this DispatcherConfiguration configuration, Assembly assembly, String[] namespaces, Action<Type> singletoneRegistrator)
        {
            configuration.DispatcherHandlerRegistry.Register(assembly, namespaces, singletoneRegistrator);
            return configuration;
        }

        public static DispatcherConfiguration AddInterceptor(this DispatcherConfiguration configuration, Type interceptor)
        {
            configuration.DispatcherHandlerRegistry.AddInterceptor(interceptor);
            return configuration;
        }

        public static DispatcherConfiguration AddHandlers(this DispatcherConfiguration configuration, Assembly assembly,Action<Type> singletoneRegistrator)
        {
            return AddHandlers(configuration, assembly, new string[] { }, singletoneRegistrator);
        }
    }
}