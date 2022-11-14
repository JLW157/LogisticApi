using LogisticApi.Models.LogisticModels;
using MongoDB.Bson.Serialization.Attributes;
using Route = LogisticApi.Models.LogisticModels.Route;

namespace LogisticApi.Models.Data.DbModels
{
    public class TransportDTO
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? TransportId { get; set; }
        [BsonRequired]
        public int WeightKg { get; set; }
        [BsonRequired]
        public int MaxPassengers { get; set; }
        [BsonRequired]
        public RouteDTO Route { get; set; }

        [BsonRequired]
        public TypeTransport TypeOfTransport { get; set; }
        [BsonRequired]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public List<string> PassengersId { get; set; }
        public enum TypeTransport
        {
            Car,
            Bus,
            Truck
        }
    }
}
