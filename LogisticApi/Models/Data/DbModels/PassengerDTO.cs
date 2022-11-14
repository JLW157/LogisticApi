using LogisticApi.Models.LogisticModels;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson.Serialization.Attributes;
using System.Runtime.CompilerServices;

namespace LogisticApi.Models.Data.DbModels
{
    public class PassengerDTO
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? PassengerId { get; set; }

        public TicketDTO? Ticket { get; set; }
    }
}
