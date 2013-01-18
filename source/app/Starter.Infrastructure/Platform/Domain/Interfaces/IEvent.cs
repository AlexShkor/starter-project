using DQF.Platform.Domain.Messages;

namespace DQF.Platform.Domain.Interfaces
{
    /// <summary>
    /// Domain Event
    /// </summary>
    public interface IEvent
    {
        string Id { get; set; }
        EventMetadata Metadata { get; set; }
    }
}