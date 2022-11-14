using LogisticApi.Models.Data.DbModels;
using LogisticApi.Models.Data.Models;
using LogisticApi.Models.LogisticModels;
using LogisticApi.Models.Requests.WaypointResponse;
using LogisticApi.Services.LogisticRepository;
using LogisticApi.Services.LogisticService;
using Microsoft.Extensions.FileProviders.Physical;
using MongoDB.Driver.Core.Operations;

namespace LogisticApi.Models.Requests.WaypointRequestBuilder
{
    public class WaypointRequestBuilder : IWaypointRequestBuilder
    {
        private readonly LogisticService service;
        private readonly TransportDTO transport;
        private WaypointRequest waypointRequest;
        public WaypointRequestBuilder(TransportDTO transport, LogisticService service)
        {
            this.transport = transport;
            this.service = service;
            this.waypointRequest = new WaypointRequest();
        }

        public IWaypointRequestBuilder BuildOptions()
        {
            waypointRequest.options = new WaypointOptions()
            {
                vehicleMaxSpeed = 90,
                vehicleWeight = transport.WeightKg,
                vehicleLength = 4,
                vehicleWidth = 2.3,
                vehicleHeight = 1.7,
                outputExtensions = new List<string>() { "travelTimes", "routeLengths" },
                traffic = "historical",
                departAt = transport.Route.DepartureTime,
                waypointConstraints = new WaypointConstraints()
                {
                    originIndex = 0,
                    destinationIndex = -1
                }
            };

            switch (transport.TypeOfTransport)
            {
                case TransportDTO.TypeTransport.Car:
                    waypointRequest.options.travelMode = "car";                    
                    break;
                case TransportDTO.TypeTransport.Bus:
                    waypointRequest.options.travelMode = "truck";                    
                    break;
                case TransportDTO.TypeTransport.Truck:
                    waypointRequest.options.travelMode = "truck";                    
                    break;
                default:
                    waypointRequest.options.travelMode = "car";                    
                    break;
            }

            return this;
        }

        public async Task<IWaypointRequestBuilder> BuildWaypointsAsync()
        {
            List<StationDTO> stations = new List<StationDTO>();
            var stationsIds = transport.Route.Stations;
            foreach (var item in stationsIds)
            {
                var res = await service.GetStation(item);
                if (res != null)
                {
                    stations.Add(res);
                }
            }

            if (stations.Count < 1)
            {
                Console.WriteLine("Error!!!!!! While Building Waypoints");
                return this;
            }

            foreach (var item in stations)
            {
                waypointRequest.waypoints.Add(new Waypoint() { point = new Point() { latitude = item.Latitude, longitude = item.Longitude } });
            }

            return this;
        }

        public WaypointRequest GetWaypointRequest()
        {
            return waypointRequest;
        }
    }
}
