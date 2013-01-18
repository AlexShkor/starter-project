using DQF.Domain.Aggregates.User.Events;

namespace DQF.Domain.Aggregates.User
{
    public class UserState
    {
        public string Id { get; private set; }
        public UserRoleEnum Role { get; private set; }

        public void On(UserCreated e)
        {
            Id = e.Id;
            Role = e.Role;
        }
    }
}