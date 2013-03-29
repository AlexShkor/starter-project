using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.ServiceLocation;
using DQF.Platform.Dispatching.Exceptions;
using DQF.Platform.Dispatching.Interfaces;

namespace DQF.Platform.Dispatching
{
    public class Dispatcher : IDispatcher
    {
        /// <summary>
        /// Service Locator that is used to create handlers
        /// </summary>
        private readonly IServiceLocator _serviceLocator;

        /// <summary>
        /// Registry of all registered handlers
        /// </summary>
        private readonly DispatcherHandlerRegistry _registry;

        /// <summary>
        /// Number of retries in case exception was logged
        /// </summary>
        private readonly int _maxRetries;

        /// <summary>
        /// Logger
        /// </summary>
        private static readonly NLog.Logger _log = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Current message that is dispatched 
        /// </summary>
        [ThreadStatic]
        public static Object CurrentMessage;      

        [ThreadStatic]
        private static bool _initialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public Dispatcher(DispatcherConfiguration configuration)
        {
            if (configuration.ServiceLocator == null)
                throw new ArgumentException("Unity Container is not registered for distributor.");

            if (configuration.DispatcherHandlerRegistry == null)
                throw new ArgumentException("Dispatcher Handler Registry is null in distributor.");

            _serviceLocator = configuration.ServiceLocator;
            _registry = configuration.DispatcherHandlerRegistry;
            _maxRetries = configuration.NumberOfRetries;
        }

        /// <summary>
        /// Factory method
        /// </summary>
        public static Dispatcher Create(Func<DispatcherConfiguration, DispatcherConfiguration> configurationAction)
        {
            var config = new DispatcherConfiguration();
            configurationAction(config);
            return new Dispatcher(config);
        }

        public void Dispatch(Object message)
        {
            Dispatch(message, null);
        }

        public void Dispatch(Object message, Action<Exception> exceptionObserver)
        {
            try
            {
                CurrentMessage = message;
                var handlersBag = _serviceLocator.GetInstance<IHandlersAgregator>();
                var handlers = handlersBag.GetHandlersFor(message.GetType());
                foreach (var handler in handlers)
                {
                    try
                    {
                        ExecuteHandler(handler, message, exceptionObserver);
                    }
                    catch (HandlerException handlerException)
                    {
                        _log.ErrorException("Message handling failed.", handlerException);
                    }
                }
            }
            catch (Exception exception)
            {
                throw new DispatchingException("Error when dispatching message", exception);
            }
        }

        private void ExecuteHandler(Action<Object> handler, Object message, Action<Exception> exceptionObserver = null)
        {
            var attempt = 0;
            while (attempt < _maxRetries)
            {
                try
                {
                    var context = new DispatcherInvocationContext(this, handler, message);
                    if (_registry.Interceptors.Count > 0)
                    {
                        // Call interceptors in backward order
                        for (int i = _registry.Interceptors.Count - 1; i >= 0; i--)
                        {
                            var interceptorType = _registry.Interceptors[i];
                            var interceptor = (IMessageHandlerInterceptor)_serviceLocator.GetInstance(interceptorType);
                            context = new DispatcherInterceptorContext(interceptor, context);
                        }
                    }
                    context.Invoke();
                    return;
                }
                catch (Exception exception)
                {
                    if (exceptionObserver != null)
                        exceptionObserver(exception);

                    attempt++;

                    if (attempt == _maxRetries)
                    {
                        throw new HandlerException(String.Format(
                            "Exception in the handler {0} for message {1}", handler.GetType().FullName, message.GetType().FullName), exception, message);

                    }
                }
            }            
        }

        public void InvokeHandler(Action<object> handler, object message)
        {
            handler.Invoke(message);
        }
    }
}
