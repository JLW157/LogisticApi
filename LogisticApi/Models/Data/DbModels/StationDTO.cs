using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace LogisticApi.Models.Data.Models
{
    public class StationDTO
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRequired]
        public double Latitude { get; set; }

        [BsonRequired]
        public double Longitude { get; set; }

        [BsonRequired]
        public string Name { get; set; } = string.Empty;
    }
}
