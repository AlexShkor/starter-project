using System.Collections.Generic;
using DQF.Platform.Domain.Interfaces;
using DQF.Platform.Domain.Messages;

namespace DQF.Domain.ApplicationServices.Emailing.Commands
{
    public class SendMail: Command
    {
        public SendMail()
        {
            
        }

        public SendMail(string recipient, string subject, string body)
        {
            Recipients = new List<string>{recipient};
            Subject = subject;
            Body = body;
        }

        public List<string> Recipients { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
    }
}