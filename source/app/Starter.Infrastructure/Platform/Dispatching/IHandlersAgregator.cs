using System;
using System.Collections.Generic;

namespace DQF.Platform.Dispatching
{
    public interface IHandlersAgregator
    {
        IEnumerable<Action<object>> GetHandlersFor(Type messageType);
    }
}