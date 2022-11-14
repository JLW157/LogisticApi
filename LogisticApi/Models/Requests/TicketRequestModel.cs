using MongoDB.Bson.Serialization.Attributes;

namespace LogisticApi.Models.Requests
{
    public class TicketRequestModel
    {
        [BsonRequired]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string PassengerId { get; set; }

        [BsonRequired]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string EndStationId { get; set; }
   
        [BsonRequired]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string StationStartId { get; set; }
    }
}
