using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;
using DQF.Domain.Aggregates.Site.Data;
using Uniform;

namespace DQF.Views
{
    public class SiteView
    {
        [DocumentId, BsonId]
        public string Id { get; set; }

        public SmtpSettingsData SmtpSettings { get; set; }

        public bool SchedulerStopped { get; set; }

        public bool SchedulerRestartNeeded { get; set; }

        public SiteView()
        {
            SmtpSettings = new SmtpSettingsData();
        }

    }
}