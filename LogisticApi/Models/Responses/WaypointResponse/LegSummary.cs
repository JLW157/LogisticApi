namespace LogisticApi.Models.Responses.WaypointResponse
{
    public class LegSummary
    {
        public int originIndex { get; set; }
        public int destinationIndex { get; set; }
        public int lengthInMeters { get; set; }
        public int travelTimeInSeconds { get; set; }
        public object departureTime { get; set; } = new object();
        public DateTime arrivalTime { get; set; }
    }
}
