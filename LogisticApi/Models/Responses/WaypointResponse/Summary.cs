namespace LogisticApi.Models.Responses.WaypointResponse
{
    public class Summary
    {
        public RouteSummary routeSummary { get; set; } = new RouteSummary();
        public List<LegSummary> legSummaries { get; set; } = new List<LegSummary>();
    }
}
