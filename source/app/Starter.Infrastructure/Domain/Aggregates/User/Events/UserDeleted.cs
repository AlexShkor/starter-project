using DQF.Platform.Domain.Messages;

namespace DQF.Domain.Aggregates.User.Events
{
    public class UserDeleted: Event
    {
        public string DeletedByUserId { get; set; }
    }
}