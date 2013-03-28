using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DQF.Platform.Dispatching;
using DQF.Platform.Domain.Interfaces;
using DQF.Platform.Domain.Transitions;

namespace DQF.Platform.Domain
{
    public class StateSpooler
    {
        private static readonly ConcurrentDictionary<MethodDescriptor, MethodInfo> _methodCache = new ConcurrentDictionary<MethodDescriptor, MethodInfo>();

        public static void Spool(Object state, IEvent evnt)
        {
            if (state == null) throw new ArgumentNullException("state");
            InvokeMethodOn(state, evnt);
        }

        public static void Spool(Object state, IEnumerable<IEvent> events)
        {
            if (state == null) throw new ArgumentNullException("state");

            foreach (var evnt in events)
                InvokeMethodOn(state, evnt);
        }

        public static void Spool(Object state, IEnumerable<Transition> transitions)
        {
            Spool(state, transitions.SelectMany(t => t.Events).Select(e => (IEvent) e.Data));
        }

        private static void InvokeMethodOn(Object state, Object message)
        {
            var methodDescriptor = new MethodDescriptor(state.GetType(), message.GetType());
            MethodInfo methodInfo = null;
            if (!_methodCache.TryGetValue(methodDescriptor, out methodInfo))
                _methodCache[methodDescriptor] = methodInfo = state.GetType()
                    .GetMethod("On", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, 
                        new[] { message.GetType() }, null);

            if (methodInfo == null)
                return;

            methodInfo.Invoke(state, new[] { message });
        }        
    }


    public struct MethodDescriptor
    {
        public readonly Type HandlerType;
        public readonly Type MessageType;

        public MethodDescriptor(Type handlerType, Type messageType)
            : this()
        {
            HandlerType = handlerType;
            MessageType = messageType;
        }

        public bool Equals(MethodDescriptor descriptor)
        {
            return descriptor.HandlerType == HandlerType && descriptor.MessageType == MessageType;
        }

        public override bool Equals(object descriptor)
        {
            if (ReferenceEquals(null, descriptor))
                return false;

            if (descriptor.GetType() != typeof(MethodDescriptor))
                return false;

            return Equals((MethodDescriptor)descriptor);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((HandlerType != null ? HandlerType.GetHashCode() : 0) * 397)
                     ^ (MessageType != null ? MessageType.GetHashCode() : 0);
            }
        }
    }
}