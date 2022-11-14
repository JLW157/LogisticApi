using LogisticApi.Models.Data.DbModels;
using LogisticApi.Models.Data.Models;
using LogisticApi.Models.LogisticModels;
using LogisticApi.Models.Requests.WaypointRequestBuilder;
using LogisticApi.Models.Responses;
using LogisticApi.Services.Builders.WaypointResponseBuilder;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Text;

namespace LogisticApi.Services.LogisticService
{
    public class LogisticService : ILogisticService
    {
        private readonly ILogisticRepository _repository;
        public LogisticService(ILogisticRepository logisticRepository)
        {
            this._repository = logisticRepository;
        }

        public async Task<Response> AddPassenger(PassengerDTO passenger)
        {
            if (passenger == null)
            {
                return new Response(false, "Value can`t be null", 400);
            }
            try
            {
                await _repository.AddPassenger(passenger);
                return new Response(true, "Passenger added succesfully", 200);
            }
            catch (Exception ex)
            {
                return new Response(false, ex.Message, 400);
            }
        }

        public async Task<Response> AddPassengerToTransport(string passengerId, string transportId)
        {
            if (await _repository.CheckPassengerAnyAsync(Builders<PassengerDTO>.Filter.Where(_ => _.PassengerId == passengerId))
                && await _repository.CheckTransportAnyAsync(Builders<TransportDTO>.Filter.Where(_ => _.TransportId == transportId)))
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


                var transportList = await _repository.FindTransports(filterTransport1);
                var transport = transportList.FirstOrDefault();

                var passengerList = await _repository.FindPassengers(filterPassenger1);
                var passenger = passengerList.FirstOrDefault();

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
                            await _repository.UpdateTransportOneAsync(filter, update);
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

        public async Task<Response> AddStation(StationDTO station)
        {
            if (station == null)
            {
                return new Response(false, "Value can`t be null", 400);
            }
            else
            {
                try
                {
                    await _repository.AddStation(station);
                }
                catch (Exception ex)
                {
                    return new Response(false, ex.Message, 400);
                }

                return new Response(true, "Station added successfully.", 200);
            }
        }

        public async Task<Response> AddStationToTransport(string stationId, string transportId)
        {
            var res = await _repository.
                FindTransports(Builders<TransportDTO>.Filter.
                Where(_ => _.TransportId == transportId));

            if (await _repository.CheckTransportAnyAsync(Builders<TransportDTO>.Filter.
                Where(_ => _.TransportId == transportId)) && 
                await _repository.CheckStationAnyAsync(Builders<StationDTO>.Filter.Where(s => s.Id == stationId)))
            {
                var filter = Builders<TransportDTO>.Filter.Eq("_id", ObjectId.Parse(transportId));
                var update = Builders<TransportDTO>.Update.
                    Push(e => e.Route.Stations, stationId);
                var filterCheck = Builders<TransportDTO>.Filter.
                    And(Builders<TransportDTO>.Filter.Eq("_id", ObjectId.Parse(transportId)),
                    Builders<TransportDTO>.Filter.Eq("Route._id", ObjectId.Parse(stationId)));
                if (await _repository.CheckTransportAnyAsync(filterCheck))
                {
                    return new Response(false, "Station has already added to this transport");
                }
                await _repository.UpdateTransportOneAsync(filter, update);
                return new Response(true, "Added successfully", 200);
            }
            return new Response(false, "Not found.", 404);
        }

        public async Task<Response> AddTransport(TransportDTO transport)
        {
            if (transport == null)
            {
                return new Response(false, "Value can`t be null", 400);
            }
            try
            {
                await _repository.AddTransport(transport);
                return new Response(true, "Transport added succesfully", 200);
            }
            catch (Exception ex)
            {
                return new Response(false, ex.Message, 400);
            }
        }

        public async Task<Response> BuyTicketForPassenger(string passengerId, string stationStartId, string endStationId)
        {
            var filterPassengerAndTicketExistsChecker = Builders<PassengerDTO>.Filter.And(
                Builders<PassengerDTO>.Filter.Eq("_id", ObjectId.Parse(passengerId)),
                Builders<PassengerDTO>.Filter.Eq("Ticket", BsonNull.Value));

            var filterStationStartExistsChecker = Builders<StationDTO>.Filter.Eq("_id", ObjectId.Parse(stationStartId));
            var filterStationEndExistsChecker = Builders<StationDTO>.Filter.Eq("_id", ObjectId.Parse(endStationId));

            var passngerFilter = Builders<PassengerDTO>.Filter.Eq("_id", ObjectId.Parse(passengerId));

            var updatePassngerWithTicket = Builders<PassengerDTO>.Update.
                Set("Ticket",
                new TicketDTO()
                {
                    StartStationId = stationStartId,
                    EndStationId = endStationId
                });

            if (await _repository.CheckPassengerAnyAsync(filterPassengerAndTicketExistsChecker))
            {
                if (await _repository.CheckStationAnyAsync(filterStationStartExistsChecker) &&
                    await _repository.CheckStationAnyAsync(filterStationEndExistsChecker))
                {
                    await _repository.UpdatePassengerOneAsync(passngerFilter, updatePassngerWithTicket);
                    return new Response(true, "Ticket successfully bought", 200);
                }
                else
                {
                    return new Response(false, "Station or Stations not found", 404);
                }
            }

            return new Response(false, "Not found or passnger have ticket", 404);
        }

        public async Task<List<StationDTO>> GetAllStations()
        {
            try
            {
                var stations = await _repository.FindStations(Builders<StationDTO>.Filter.Where(_ => true));
                return stations;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Task<StationDTO>? GetStation(string stationId)
        {
            var station = _repository.GetStation(stationId);

            if (station == null)
            {
                return null;
            }

            return station;
        }

        public async Task<Response> RemovePassengerFromTransport(string passengerId, string transportId)
        {
            if (await _repository.CheckPassengerAnyAsync(Builders<PassengerDTO>.Filter.Where(_ => _.PassengerId == passengerId))
               && await _repository.CheckTransportAnyAsync(Builders<TransportDTO>.Filter.Where(_ => _.TransportId == transportId)))
            {
                var filter = Builders<TransportDTO>.Filter.Eq("_id", ObjectId.Parse(transportId));
                var update = Builders<TransportDTO>.Update.Pull(e => e.PassengersId, passengerId);

                await _repository.UpdateTransportOneAsync(filter, update);
                return new Response(true, "Deleted successfully", 200);
            }
            return new Response(false, "Not found.", 404);
        }

        public async Task<Response> RemoveStationFromTransport(string stationId, string transportId)
        {
            if (await _repository.CheckTransportAnyAsync(Builders<TransportDTO>.Filter.Where(_ => _.TransportId == transportId)) &&
                await _repository.CheckStationAnyAsync(Builders<StationDTO>.Filter.Where(_ => _.Id == stationId)))
            {
                var filter = Builders<TransportDTO>.Filter.Eq("_id", ObjectId.Parse(transportId));
                var update = Builders<TransportDTO>.Update.
                    Pull(e => e.Route.Stations, stationId);

                await _repository.UpdateTransportOneAsync(filter, update);
                return new Response(true, "Deleted successfully", 200);
            }
            return new Response(false, "Not found.", 404);
        }

        public async Task<ApiResponse> StartRace(string transportId)
        {
            var filterFindTransport = Builders<TransportDTO>.Filter.Eq("_id", ObjectId.Parse(transportId));

            var transportSearch = await _repository.FindTransports(filterFindTransport);

            if (transportSearch.Count() < 1)
            {
                return new ApiResponse(false, "Transport not found.", 404);
            }

            var transport = transportSearch.FirstOrDefault();

            if (transport.Route.Stations.Count < 1)
            {
                return new ApiResponse(false, "Route does not have any stations", 400);
            }

            var builder = new WaypointRequestBuilder(transport, this);

            var request = (await builder.BuildWaypointsAsync()).BuildOptions().GetWaypointRequest();

            if (request == null)
            {
                return new ApiResponse(false, "Something went wrong", 400);
            }

            var url = "https://api.tomtom.com/routing/waypointoptimization/1?key=jAuU8NSwxN6583OC4GTatboHYLeFGUex";
            using (var client = new HttpClient())
            {
                var postObj = request;

                var postJson = JsonConvert.SerializeObject(postObj);
                StringContent content = new StringContent(postJson, Encoding.UTF8, "application/json");
                var result = await client.PostAsync(url, content);
                if (result.IsSuccessStatusCode)
                {
                    var json = await result.Content.ReadAsStringAsync();

                    WaypointReportBuilder reportBuilder = new WaypointReportBuilder(json);

                    var report = reportBuilder.
                        BuildLegSummary().
                        BuildOptimizedOrder().
                        BuildRouteSummmary().
                        GetWaypointReport();

                    return new ApiResponse(true, "Success", 200, report);
                }
                else
                {
                    return new ApiResponse(false, "Something went wrong :(", 400);
                }
            }
        }
    }
}
