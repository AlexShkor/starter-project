using System;
using DQF.Platform.Domain.Messages;

namespace DQF.Domain.Aggregates.User.Commands
{
    public class CreateUser : Command
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRoleEnum Role { get; set; }
        public string Phone { get; set; }
    }
}