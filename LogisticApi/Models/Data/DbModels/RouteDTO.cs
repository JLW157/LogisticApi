using LogisticApi.Models.Data.Models;
using LogisticApi.Models.LogisticModels;
using MongoDB.Bson.Serialization.Attributes;

namespace LogisticApi.Models.Data.DbModels
{
    public class RouteDTO
    {
        [BsonRequired]
        public string RouteName { get; set; }

        [BsonRequired]
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public List<string> Stations { get; set; }
        [BsonRequired]
        public DateTime DepartureTime { get; set; }
    }
}
