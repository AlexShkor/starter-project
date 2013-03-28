using System;
using DQF.Domain.Aggregates.User;
using MongoDB.Bson;
using DQF.Domain.Aggregates.Site.Commands;
using DQF.Domain.Aggregates.Site.Data;
using DQF.Domain.Aggregates.User.Commands;
using DQF.Platform.Domain;

namespace DQF.Helpers
{

    public class DatabaseGenerator
    {
        public const string AdminUserName = "admin1";
        private readonly ICommandBus _bus;

        public DatabaseGenerator(ICommandBus bus)
        {
            _bus = bus;
        }

        public void SetupInitialData()
        {
            CreateSite();
            CreateAdmin();
        }

        private void CreateSite()
        {
            var createSite = new CreateSite {SiteId = SiteSettings.SiteId };
            _bus.Send(createSite);
            _bus.Send(new SetSiteSettings
            {
                SiteId = createSite.SiteId,
                SmtpSettings = new SmtpSettingsData
                {
                    SmtpServer = "smtp.sendgrid.net",
                    SmtpServerPort = 587,
                    Email = "no-reply@dqf.com",
                    Username = "dqf",
                    Password = "m00re$c0al",
                }
            });
        }

        private void CreateAdmin()
        {
            var cmd = new CreateUser
            {
                UserName = AdminUserName,
                Email = AdminUserName,
                Password = "firstlogin",
                Role = UserRoleEnum.GlobalAdmin,
            };

            _bus.Send(cmd);
        }
    }
}