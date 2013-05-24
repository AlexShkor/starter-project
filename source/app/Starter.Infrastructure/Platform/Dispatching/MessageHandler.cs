using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using DQF.Platform.Dispatching.Attributes;
using DQF.Platform.Dispatching.Interfaces;
using DQF.Platform.Dispatching.Utils;

namespace DQF.Platform.Dispatching
{
    public abstract class MessageHandler : IMessageHandler
    {
        private int _proirity = 0;

        protected void SetPriority(int priority)
        {
            _proirity = priority;
        }

        private readonly ConcurrentDictionary<Type, HandlerActionInfo> _handlers;

        protected MessageHandler()
        {
            _handlers = new ConcurrentDictionary<Type, HandlerActionInfo>();
        }

        protected void Handle<TMessage>(Action<TMessage> handler)
        {
           // var priorityAttribute = ReflectionUtils.GetSingleAttribute<PriorityAttribute>(this.GetType());
           // var defaultPriority = priorityAttribute == null ? 0 : priorityAttribute.Priority;
            Handle(handler, _proirity);
        }

        protected void Handle<TMessage>(Action<TMessage> handler, int priority)
        {
            var type = typeof(TMessage);
            _handlers[type] = new HandlerActionInfo
            {
                MessageType = type,
                HandlerType = this.GetType(),
                Priority = priority,
                Handler = o =>
                {
                    var message = (TMessage) o;
                    handler(message);
                }
            };
        }

        public bool HasHandlerFor(Type messageType)
        {
            return _handlers.ContainsKey(messageType);
        }

        public HandlerActionInfo GetHandlerFor(Type messageType)
        {
            return _handlers[messageType];
        }

        public IEnumerable<HandlerActionInfo> GetHandlerActions()
        {
            return _handlers.Values;
        }
    }

    public struct HandlerActionInfo
    {
        public int Priority { get; set; }
        public Type MessageType { get; set; }
        public Type HandlerType { get; set; }
        public Action<object> Handler { get; set; }
    }
}