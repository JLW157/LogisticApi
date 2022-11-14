namespace LogisticApi.Models.Responses.WaypointResponse
{
    public class RouteSummary
    {
        public int originIndex { get; set; }
        public int destinationIndex { get; set; }
        public int lengthInMeters { get; set; }
        public int travelTimeInSeconds { get; set; }
        public DateTime departureTime { get; set; }
        public DateTime arrivalTime { get; set; }
    }
}
