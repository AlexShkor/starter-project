using System;
using System.Collections;
using System.Collections.Generic;

namespace DQF.Platform.Dispatching.Interfaces
{
    public interface IMessageHandler
    {
        IEnumerable<HandlerActionInfo> GetHandlerActions();
        HandlerActionInfo GetHandlerFor(Type messageType);
        bool HasHandlerFor(Type messageType);
    }
}