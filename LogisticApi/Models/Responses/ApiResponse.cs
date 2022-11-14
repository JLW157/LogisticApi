using LogisticApi.Models.Requests;
using LogisticApi.Models.Responses;
using LogisticApi.Models.Responses.WaypointResponse;

namespace LogisticApi.Models.Responses
{
    public class ApiResponse
    {
        public ApiResponse(bool isSuccess, string message, int statusCode)
        {
            Message = message;
            IsSuccess = isSuccess;
            StatusCode = statusCode;
        }

        public ApiResponse(bool isSuccess, string message, int statusCode, WaypointReport waypointReport)
        {
            Message = message;
            IsSuccess = isSuccess;
            StatusCode = statusCode;
            WaypointReport = waypointReport;
        }

        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }

        public WaypointReport WaypointReport { get; set; }

    }
}
