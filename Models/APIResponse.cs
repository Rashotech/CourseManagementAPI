using System.Net;

namespace CourseManagement.Models
{
    public class APIResponseBase
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        public string Message { get; set; } = string.Empty;
        public bool IsSuccess { get; set; } = true;
        public object? Data { get; set; } = null;
    }

    public class APIResponse<T> : APIResponseBase
    {
        public new T? Data { get; set; }
    }
}