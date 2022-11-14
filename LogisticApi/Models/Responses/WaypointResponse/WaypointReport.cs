namespace LogisticApi.Models.Responses.WaypointResponse
{
    public class WaypointReport
    {
        public List<int> optimizedOrder { get; set; } = new List<int>();
        public Summary summary { get; set; } = new Summary();
    }
}
