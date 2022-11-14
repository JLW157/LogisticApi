namespace LogisticApi.Models.Requests.WaypointResponse
{
    public class WaypointOptions
    {
        public string travelMode { get; set; }
        public int vehicleMaxSpeed { get; set; }
        public int vehicleWeight { get; set; }
        public int vehicleLength { get; set; }
        public double vehicleWidth { get; set; }
        public double vehicleHeight { get; set; }
        public List<string> outputExtensions { get; set; }
        public string traffic { get; set; }
        public DateTime departAt { get; set; }
        public WaypointConstraints waypointConstraints { get; set; }
    }
}
