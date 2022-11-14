using LogisticApi.Models.Responses.WaypointResponse;

namespace LogisticApi.Models.Requests.WaypointRequestBuilder
{
    public interface IWaypointRequestBuilder
    {
        Task<IWaypointRequestBuilder> BuildWaypointsAsync();
        IWaypointRequestBuilder BuildOptions();
        public WaypointRequest GetWaypointRequest();
    }
}
