using DQF.Platform.Domain.Messages;

namespace DQF.Domain.Aggregates.User.Commands
{
    public class UserCommand : Command
    {
        public string UserId { get; set; }
    }
}