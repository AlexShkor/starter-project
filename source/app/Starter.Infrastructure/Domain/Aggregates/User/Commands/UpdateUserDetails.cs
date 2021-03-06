﻿using System.Collections.Generic;
using DQF.Platform.Domain.Messages;

namespace DQF.Domain.Aggregates.User.Commands
{
    public class UpdateUserDetails : UserCommand
    {
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public List<string> AdditionalEmails { get; set; }
    }
}