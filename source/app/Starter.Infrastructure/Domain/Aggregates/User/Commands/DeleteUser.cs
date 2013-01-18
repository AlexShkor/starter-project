using DQF.Platform.Domain.Messages;

namespace DQF.Domain.Aggregates.User.Commands
{
    public class DeleteUser: Command
    {
        public string DeletedByUserId { get; set; }
    }
}