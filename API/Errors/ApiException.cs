namespace API.Errors
{
    public class ApiException
    {
        // we always want to get d statusCode as an argument from any class dt create an instance of ds ApiException but the message and details are optional so we give them a default value of null..
        public ApiException(int statusCode, string message = null, string details = null)
        {
            StatusCode = statusCode;
            Message = message;
            Details = details;
        }
        // d error status code e.g. 400, 401, 500
        public int StatusCode { get; set; }
        // d error msg to use e.g. "Computer says No"
        public string Message { get; set; }
        // d error stack trace
        public string Details { get; set; }
    }
}
