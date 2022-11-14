using LogisticApi.Models.Requests.WaypointResponse;

namespace LogisticApi.Models.Requests
{
    public class WaypointRequest
    {
        public List<Waypoint> waypoints { get; set; } = new List<Waypoint>();
        public WaypointOptions options { get; set; } = null!;
    }
}
