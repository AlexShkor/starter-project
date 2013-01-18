using DQF.Platform.Settings;

namespace DQF
{
    public class SiteSettings
    {
        public const string SiteId = "DQF-Site-2";

        [SettingsProperty("mongo.events_connection_string")]
        public string MongoEventsConnectionString { get; set; }

        [SettingsProperty("mongo.views_connection_string")]
        public string MongoViewConnectionString { get; set; }
            
        [SettingsProperty("mongo.logs_connection_string")]
        public string MongoLogsConnectionString { get; set; }

        [SettingsProperty("HtmlToPdfToolPath")]
        public string HtmlToPdfToolPath { get; set; }

        [SettingsProperty("google.api_key")]
        public string GoogleApiKey { get; set; }

        [SettingsProperty("sendgrid.username")]
        public string SendgridUsername { get; set; }

        [SettingsProperty("sendgrid.password")]
        public string SendgridPassword { get; set; }

        [SettingsProperty("AmazonSecretKey")]
        public string AmazonSecretKey { get; set; }
        [SettingsProperty("AmazonAccessKey")]
        public string AmazonAccessKey { get; set; }
        public string UploadDir { get; set; }
    }
}