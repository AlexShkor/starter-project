using System;
using DQF.Platform.Domain.Messages;

namespace DQF.Domain.Aggregates.User.Commands
{
    public class ChangePassword: UserCommand
    {
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public DateTime Date { get; set; }
        public bool IsChangedByAdmin { get; set; }
    }
}