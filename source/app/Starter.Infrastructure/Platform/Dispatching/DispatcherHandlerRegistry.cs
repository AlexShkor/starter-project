using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DQF.Platform.Dispatching.Exceptions;
using DQF.Platform.Dispatching.Interfaces;

namespace DQF.Platform.Dispatching
{
    public class DispatcherHandlerRegistry
    {
        private readonly Type _markerInterface = typeof(IMessageHandler);

        /// <summary>
        /// Message interceptors
        /// </summary>
        private readonly List<Type> _interceptors = new List<Type>();

        /// <summary>
        /// Message interceptors
        /// </summary>
        public List<Type> Interceptors
        {
            get { return _interceptors; }
        }

        /// <summary>
        /// Register all handlers in assembly (you can register handlers that optionally belongs to specified namespaces)
        /// </summary>
        public void Register(Assembly assembly, String[] namespaces, Action<Type> singletoneRegistrator)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (!BelongToNamespaces(type, namespaces) || type.IsAbstract)
                    continue;

                var interfaces = type.GetInterfaces();
                var markerInterface = interfaces.FirstOrDefault(i => i == _markerInterface);

                if (markerInterface != null)
                {
                    singletoneRegistrator(type);
                }
            }
        }

        public void AddInterceptor(Type type)
        {
            if (!typeof(IMessageHandlerInterceptor).IsAssignableFrom(type))
                throw new Exception(String.Format("Interceptor {0} must implement IMessageHandlerInterceptor", type.FullName));

            if (_interceptors.Contains(type))
                throw new Exception(String.Format("Interceptor {0} already registered", type.FullName));

            _interceptors.Add(type);
        }

        private Boolean BelongToNamespaces(Type type, String[] namespaces)
        {
            // if no namespaces specified - then type belong to any namespace
            if (namespaces.Length == 0)
                return true;

            foreach (var ns in namespaces)
            {
                if (type.FullName.StartsWith(ns))
                    return true;
            }

            return false;
        }
    }
}
