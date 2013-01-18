using System.Collections.Generic;
using DQF.Platform.Domain.Interfaces;

namespace DQF.Platform.Domain.EventBus
{
    public interface IEventBus
    {
        void Publish(IEvent eventMessage);
        void Publish(IEnumerable<IEvent> eventMessages);
    }
}