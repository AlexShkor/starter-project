using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using DQF.Platform.Dispatching.Interfaces;

namespace DQF.Platform.Dispatching
{
    public class HandlersAgregator: IHandlersAgregator
    {
        private readonly Dictionary<Type,SortedList<int, List<HandlerActionInfo>>> _handlerActions;

        public HandlersAgregator(IEnumerable<IMessageHandler> handlers)
        {
            _handlerActions = new Dictionary<Type, SortedList<int, List<HandlerActionInfo>>>();
            foreach (var messageHandler in handlers)
            {
                foreach (var handlerAction in messageHandler.GetHandlerActions())
                {
                    SetFor(handlerAction);
                }
            }
        }

        private void SetFor(HandlerActionInfo handlerActionInfo)
        {
            if (!_handlerActions.ContainsKey(handlerActionInfo.MessageType))
            {
                _handlerActions[handlerActionInfo.MessageType] = new SortedList<int, List<HandlerActionInfo>>();
            }
            if (!_handlerActions[handlerActionInfo.MessageType].ContainsKey(handlerActionInfo.Priority))
            {
                _handlerActions[handlerActionInfo.MessageType][handlerActionInfo.Priority] = new List<HandlerActionInfo>();
            }
            _handlerActions[handlerActionInfo.MessageType][handlerActionInfo.Priority].Add(handlerActionInfo);
        }

        public  IEnumerable<Action<object>> GetHandlersFor(Type messageType)
        {
            if (!_handlerActions.ContainsKey(messageType))
            {
                yield break;
            }
            foreach (var handler in _handlerActions[messageType].Values)
            {
                foreach (var action in handler)
                {
                    yield return action.Handler;
                }
            }
        }
    }
}