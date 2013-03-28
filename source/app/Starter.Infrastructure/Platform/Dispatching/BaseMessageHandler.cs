using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using DQF.Platform.Dispatching.Attributes;
using DQF.Platform.Dispatching.Utils;

namespace DQF.Platform.Dispatching
{
    public abstract class MessageHandler : IMessageHandler
    {
        private static readonly ConcurrentDictionary<Type, SortedList<int, Action<object>>> Handlers = new ConcurrentDictionary<Type, SortedList<int, Action<object>>>();
        private static readonly ConcurrentBag<Type> InitializedTypes = new ConcurrentBag<Type>();

        protected void Handle<TMessage>(Action<TMessage> handler)
        {
            var priorityAttribute = ReflectionUtils.GetSingleAttribute<PriorityAttribute>(this.GetType());
            var defaultPriority = priorityAttribute == null ? 0 : priorityAttribute.Priority;
            Handle(handler, defaultPriority);
        }

        protected void Handle<TMessage>(Action<TMessage> handler, int priority)
        {
            var type = typeof(TMessage);
            var thisType = this.GetType();
            if (InitializedTypes.Contains(thisType))
            {
                return;
            }
            InitializedTypes.Add(thisType);
            if (!Handlers.ContainsKey(type))
            {
                Handlers[type] = new SortedList<int, Action<object>>();
            }
            Handlers[type].Add(priority,o=>
            {
                var message = (TMessage) o;
                handler(message);
            });
        }

        public static IEnumerable<Action<object>>  GetHandlersFor(Type messageType)
        {
            if (!Handlers.ContainsKey(messageType))
            {
                yield break;
            }
            foreach (var handler in Handlers[messageType].Values)
            {
                yield return handler;
            }
        }

        //public static void Dispatch(Object message, Action<Exception>)
        //{
        //    var handlers = Handlers[message.GetType()];
        //    foreach (var handler in handlers)
        //    {
        //        handler.Value(message);
        //    }
        //}
    }
}