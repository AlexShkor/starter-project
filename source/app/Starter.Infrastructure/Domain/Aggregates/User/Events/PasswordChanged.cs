using DQF.Platform.Domain;
using System;
using DQF.Platform.Domain.Messages;

namespace DQF.Domain.Aggregates.User.Events
{
    public class PasswordChanged : Event
    {
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public DateTime Date { get; set; }
        public bool WasChangedByAdmin { get; set; }
    }
}