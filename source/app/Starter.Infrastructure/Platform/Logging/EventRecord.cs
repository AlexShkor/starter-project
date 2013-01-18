using MongoDB.Bson;
using DQF.Platform.Domain;
using DQF.Platform.Domain.Messages;
using DQF.Platform.Mongo;

namespace DQF.Platform.Logging
{
    public class EventRecord
    {
        public BsonDocument EventDocument { get; set; }
        public EventMetadata Metadata { get; set; }
        public EventHandlerRecordCollection Handlers { get; set; }

        public static EventRecord FromBson(BsonDocument doc)
        {
            var eventDocument = doc.GetBsonDocument("Event");
            var record = new EventRecord()
            {
                EventDocument = eventDocument,
                Metadata = eventDocument.GetBsonDocument("Metadata").CreateEventMetadata(),
                Handlers = EventHandlerRecordCollection.FromBson(doc.GetBsonArray("Handlers"))
            };

            return record;
        }  
    }
}