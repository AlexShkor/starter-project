using DQF.Domain.ApplicationServices.Emailing.Commands;
using DQF.Platform.Dispatching;
using DQF.Platform.Dispatching.Interfaces;
using DQF.Platform.Utilities;

namespace DQF.Domain.ApplicationServices.Emailing
{
    public class EmailApplicationService: MessageHandler
    {
        private readonly SendGridUtil _sendGrid;

        public EmailApplicationService(SendGridUtil sendGrid)
        {
            _sendGrid = sendGrid;
            Handle((SendMail c)=> _sendGrid.SendMessage(c.Recipients, c.Subject, c.Body));
        }
    }
}