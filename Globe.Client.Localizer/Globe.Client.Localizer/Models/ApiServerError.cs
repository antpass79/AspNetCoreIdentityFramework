using System.Net;

namespace Globe.Client.Localizer.Models
{
    public class ApiServerError
    {
        public string Message { get; set; }
        public string Details { get; set; }
        public string StackTrace { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
