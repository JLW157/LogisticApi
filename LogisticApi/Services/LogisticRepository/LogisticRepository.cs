using LogisticApi.Models.Data.DbModels;
using LogisticApi.Models.Data.Models;
using LogisticApi.Models.LogisticModels;
using LogisticApi.Models.Requests;
using LogisticApi.Models.Requests.WaypointRequestBuilder;
using LogisticApi.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using System.Text;
using Newtonsoft.Json;
using JsonConvert = Newtonsoft.Json.JsonConvert;
using LogisticApi.Models.DbOptions;
using LogisticApi.Models.Responses.WaypointResponse;
using LogisticApi.Services.Builders.WaypointResponseBuilder;
using System.Xml.Serialization;
using MongoDB.Driver.Core.Operations;
using System.Formats.Asn1;
using Microsoft.VisualBasic;
using Microsoft.Extensions.Logging.TraceSource;

namespace LogisticApi.Services.LogisticRepository
{
    public class LogisticRepository : ILogisticRepository
    {
        private readonly IMongoCollection<StationDTO> stationCollection;
        private readonly IMongoCollection<TransportDTO> transportCollection;
        private readonly IMongoCollection<PassengerDTO> passengerCollection;
        public LogisticRepository(IOptions<LogisticDatabaseOptions> opts)
        {
            var client = new MongoClient(
                opts.Value.ConnectionString);

            var mongoDatabase = client.GetDatabase(opts.Value.DatabaseName);

            stationCollection = mongoDatabase.GetCollection<StationDTO>("Stations");
            passengerCollection = mongoDatabase.GetCollection<PassengerDTO>("Passengers");
            transportCollection = mongoDatabase.GetCollection<TransportDTO>("Transports");
        }

        public async Task AddStation(StationDTO station)
        {
            await stationCollection.InsertOneAsync(station);
        }

        public async Task<StationDTO>? GetStation(string stationId)
        {
            var filter = Builders<StationDTO>.Filter.Where(s => s.Id == stationId);

            var station = await stationCollection.Find(filter).ToListAsync();

            return station.FirstOrDefault();
        }

        public async Task AddTransport(TransportDTO transport)
        {
            await transportCollection.InsertOneAsync(transport);
        }

        public async Task AddPassenger(PassengerDTO passenger)
        {
            await passengerCollection.InsertOneAsync(passenger);
        }

        public async Task<Response> AddPassengerToTransport(string passengerId, string transportId)
        {
            if (await passengerCollection.Find(_ => _.PassengerId == passengerId).AnyAsync()
                && await transportCollection.Find(_ => _.TransportId == transportId).AnyAsync())
            {
                var filter = Builders<TransportDTO>.Filter.Eq("_id", ObjectId.Parse(transportId));

                var filterCheck = Builders<TransportDTO>.Filter.
                    And(Builders<TransportDTO>.Filter.Eq("_id", ObjectId.Parse(transportId)),
                    Builders<TransportDTO>.Filter.Eq("PassengersId", ObjectId.Parse(passengerId)));

                var filterForTicketIsNullCheck = Builders<PassengerDTO>.Filter.And(
                    Builders<PassengerDTO>.Filter.Eq("_id", ObjectId.Parse(passengerId)),
                    Builders<PassengerDTO>.Filter.Eq("Ticket", BsonNull.Value));

                var filterTransport1 = Builders<TransportDTO>.Filter.Where(t => t.TransportId == transportId);
                var filterPassenger1 = Builders<PassengerDTO>.Filter.Where(p => p.PassengerId == passengerId);


                var transport = await transportCollection.Find(filterTransport1).FirstOrDefaultAsync();
                var passenger = await passengerCollection.Find(filterPassenger1).FirstOrDefaultAsync();
                var update = Builders<TransportDTO>.Update.Push(e => e.PassengersId, passengerId);

                if (passenger == null || transport == null)
                {
                    return new Response(false, "Not found.", 404);
                }
                else
                {
                    if (transport.PassengersId.Where(pId => pId == passengerId).Any())
                    {
                        return new Response(false, "Passenger has already added to this transport", 400);
                    }
                    else
                    {
                        if (passenger.Ticket == null)
                        {
                            return new Response(false, "Passenger does not have a ticket.", 400);
                        }

                        if (transport.PassengersId.Count + 1 > transport.MaxPassengers)
                        {
                            return new Response(false, "Transport does not have any free places", 400);
                        }

                        if (transport.Route.Stations.Where(sId => sId == passenger.Ticket.StartStationId).Any()
                            && transport.Route.Stations.Where(sId => sId == passenger.Ticket.EndStationId).Any())
                        {
                            await transportCollection.UpdateOneAsync(filter, update);
                            return new Response(true, "Added successfully", 200);
                        }
                        else
                        {
                            return new Response(false, "Ticket is not coorrect! Route does not have this Start Station", 400);
                        }

                    }
                }
            }
            return new Response(false, "Not found.", 404);
        }

        //Finds
        public async Task<List<TransportDTO>> FindTransports(FilterDefinition<TransportDTO> filterDefinition)
        {
            return await transportCollection.Find(filterDefinition).ToListAsync();
        }

        public async Task<bool> CheckTransportAnyAsync(FilterDefinition<TransportDTO> filterDefinition)
        {
            return await transportCollection.FindAsync(filterDefinition).Result.AnyAsync();
        }

        public async Task<bool> CheckStationAnyAsync(FilterDefinition<StationDTO> filterDefinition)
        {
            return await stationCollection.FindAsync(filterDefinition).Result.AnyAsync();
        }

        public async Task<bool> CheckPassengerAnyAsync(FilterDefinition<PassengerDTO> filterDefinition)
        {
            return await passengerCollection.FindAsync(filterDefinition).Result.AnyAsync();
        }

        public async Task<List<StationDTO>> FindStations(FilterDefinition<StationDTO> filterDefinition)
        {
            return await stationCollection.Find(filterDefinition).ToListAsync();
        }

        public async Task<List<PassengerDTO>> FindPassengers(FilterDefinition<PassengerDTO> filterDefinition)
        {
            return await passengerCollection.Find(filterDefinition).ToListAsync();
        }

        // Updates
        public async Task UpdateTransportOneAsync(FilterDefinition<TransportDTO> filterDefinition, UpdateDefinition<TransportDTO> updateDefinition)
        {
            await transportCollection.UpdateOneAsync(filterDefinition, updateDefinition);
        }

        public async Task UpdateStationOneAsync(FilterDefinition<StationDTO> filterDefinition, UpdateDefinition<StationDTO> updateDefinition)
        {
            await stationCollection.UpdateOneAsync(filterDefinition, updateDefinition);
        }

        public async Task UpdatePassengerOneAsync(FilterDefinition<PassengerDTO> filterDefinition, UpdateDefinition<PassengerDTO> updateDefinition)
        {
            await passengerCollection.UpdateOneAsync(filterDefinition, updateDefinition);
        }


    }
}
