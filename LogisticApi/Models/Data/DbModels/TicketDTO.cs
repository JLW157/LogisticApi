using MongoDB.Bson.Serialization.Attributes;

namespace LogisticApi.Models.Data.DbModels
{
    public class TicketDTO
    {
        [BsonRequired]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string StartStationId { get; set; }
        [BsonRequired]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string EndStationId { get; set; }
    }
}
