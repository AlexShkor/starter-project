namespace DQF.Domain.Aggregates.User.Commands
{
    public class DeleteUser : UserCommand
    {
        public string DeletedByUserId { get; set; }
    }
}