using System;
using DQF.Platform.Domain.Messages;

namespace DQF.Domain.Aggregates.User.Commands
{
    public class CreateUser : Command
    {
        public new string Id
        {
            get { throw new InvalidOperationException(); }
            set { throw new InvalidOperationException(); }
        }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRoleEnum Role { get; set; }
        public string Phone { get; set; }
    }
}