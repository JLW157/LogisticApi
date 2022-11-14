using LogisticApi.Models.Data.DbModels;
using LogisticApi.Models.Data.Models;
using LogisticApi.Models.Responses;

namespace LogisticApi.Services.LogisticService
{
    public interface ILogisticService
    {
        Task<List<StationDTO>> GetAllStations();
        Task<Response> AddStation(StationDTO station);
        Task<StationDTO>? GetStation(string stationId);

        Task<Response> AddPassenger(PassengerDTO transport);
        Task<Response> BuyTicketForPassenger(string passengerId, string stationStartId, string endStationId);

        Task<Response> AddTransport(TransportDTO transport);
        Task<Response> AddPassengerToTransport(string passengerId, string transportId);
        Task<Response> RemovePassengerFromTransport(string passengerId, string transportId);
        Task<Response> AddStationToTransport(string stationId, string transportId);
        Task<Response> RemoveStationFromTransport(string stationId, string transportId);

        Task<ApiResponse> StartRace(string transportId);
    }
}
