using System;
using MongoDB.Bson;

namespace DQF.Platform.Mongo
{
    /// <summary>
    /// THIS TRICKY CLASS IS NEEDED ONLY BECAUSE OF NANCY WEBFORMS AUTHENTICATION REQUIRE GUIDS
    /// It can't be used as real converter for any Guid as there is no strong corRelation between Guid and ObjectId
    /// So the only correct convertion scenario is ObjectId -> "bad" Guid -> ObjectId, as ObjectId is "less" than Guid
    /// </summary>
    public class ObjectIdConverter
    {
        private const int ObjectIdLength = 24;
        // last 8 meaningless characters that will be removed during back convertion
        private const string DummyPostfix = "00000000";

        public static Guid ToGuid(string objectId)
        {
            if (objectId.Length != ObjectIdLength)
                throw new ArgumentException();

            return new Guid(objectId + DummyPostfix);
        }

        public static ObjectId FromGuid(Guid guid)
        {
            return new ObjectId(guid.ToString().Replace("-", "").Substring(0, ObjectIdLength));
        }
    }
}