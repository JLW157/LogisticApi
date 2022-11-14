using LogisticApi.Models.Data.DbModels;
using LogisticApi.Models.Data.Models;
using LogisticApi.Models.LogisticModels;
using LogisticApi.Models.Responses;
using MongoDB.Driver;

namespace LogisticApi.Services
{
    public interface ILogisticRepository
    {
        //Station 
        Task AddStation(StationDTO station);
        Task<StationDTO>? GetStation(string stationId);
        Task<List<StationDTO>> FindStations(FilterDefinition<StationDTO> filterDefinition);
        Task UpdateStationOneAsync(FilterDefinition<StationDTO> filterDefinition, UpdateDefinition<StationDTO> updateDefinition);
        Task<bool> CheckStationAnyAsync(FilterDefinition<StationDTO> filterDefinition);

        //Transport
        Task AddTransport(TransportDTO transport);
        Task<List<TransportDTO>> FindTransports(FilterDefinition<TransportDTO> filterDefinition);
        Task UpdateTransportOneAsync(FilterDefinition<TransportDTO> filterDefinition, UpdateDefinition<TransportDTO> updateDefinition);
        Task<bool> CheckTransportAnyAsync(FilterDefinition<TransportDTO> filterDefinition);

        //Passenger
        Task AddPassenger(PassengerDTO transport);
        Task<List<PassengerDTO>> FindPassengers(FilterDefinition<PassengerDTO> filterDefinition);
        Task UpdatePassengerOneAsync(FilterDefinition<PassengerDTO> filterDefinition, UpdateDefinition<PassengerDTO> updateDefinition);
        Task<bool> CheckPassengerAnyAsync(FilterDefinition<PassengerDTO> filterDefinition);
    }
}
