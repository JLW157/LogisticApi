using MongoDB.Bson.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using JsonConvert = Newtonsoft.Json.JsonConvert;
using LogisticApi.Models.Responses;
using LogisticApi.Models.Responses.WaypointResponse;

namespace LogisticApi.Services.Builders.WaypointResponseBuilder
{
    public class WaypointReportBuilder : IWaypointReportBuilder
    {
        private readonly string json;
        private readonly JObject response;
        private readonly WaypointReport waypointReport;
        public WaypointReportBuilder(string json)
        {
            this.json = json;
            this.response = JsonConvert.DeserializeObject<JObject>(this.json);
            this.waypointReport = new WaypointReport();
        }

        public IWaypointReportBuilder BuildOptimizedOrder()
        {
            if (response.ContainsKey("optimizedOrder"))
            {
                waypointReport.optimizedOrder = JsonConvert.
                    DeserializeObject<List<int>>(response["optimizedOrder"].ToString());
                return this;
            }

            Console.WriteLine($"[{DateTime.Now}] OptimizedOrder object not found");
            waypointReport.optimizedOrder = null;
            return this;
        }

        public IWaypointReportBuilder BuildRouteSummmary()
        {
            if (response["summary"]?["routeSummary"] != null)
            {
                waypointReport.summary.routeSummary= JsonConvert.
                    DeserializeObject<RouteSummary>(response["summary"]["routeSummary"].ToString());

                return this;
            }

            Console.WriteLine($"[{DateTime.Now}] RouteSummary object not found");
            waypointReport.summary.routeSummary = null;
            return this;
        }

        public IWaypointReportBuilder BuildLegSummary()
        {
            if (response["summary"]?["legSummaries"] != null)
            {
                waypointReport.summary.legSummaries = JsonConvert.
                    DeserializeObject<List<LegSummary>>(response["summary"]["legSummaries"].ToString());

                return this;
            }

            Console.WriteLine($"[{DateTime.Now}] LegSummary object not found");
            waypointReport.summary.legSummaries = null;
            return this;
        }

        public WaypointReport GetWaypointReport()
        {
            return waypointReport;
        }
    }
}
