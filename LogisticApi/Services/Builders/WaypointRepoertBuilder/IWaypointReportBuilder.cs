using LogisticApi.Models.Responses.WaypointResponse;
using System.Security.Cryptography.X509Certificates;

namespace LogisticApi.Services.Builders.WaypointResponseBuilder
{
    public interface IWaypointReportBuilder
    {
        IWaypointReportBuilder BuildOptimizedOrder();
        IWaypointReportBuilder BuildRouteSummmary();
        IWaypointReportBuilder BuildLegSummary();

        WaypointReport GetWaypointReport();
    }
}
