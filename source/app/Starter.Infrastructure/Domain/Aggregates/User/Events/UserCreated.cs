using System;
using DQF.Platform.Domain;
using DQF.Platform.Domain.Messages;

namespace DQF.Domain.Aggregates.User.Events
{
    public class UserCreated : Event
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public UserRoleEnum Role { get; set; }
    }
}