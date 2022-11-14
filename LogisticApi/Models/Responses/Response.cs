namespace LogisticApi.Models.Responses
{
    public class Response
    {
        public Response(bool isSuccess, string message)
        {
            this.IsSuccess = isSuccess;
            this.Message = message;
        }

        public Response(bool isSuccess, string message, int statusCode)
        {
            Message = message;
            IsSuccess = isSuccess;
            StatusCode = statusCode;
        }

        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; } 

    }
}
