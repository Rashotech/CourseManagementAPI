using System.Net;

namespace CourseManagement.Models
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        public required string Message { get; set; }
		public bool IsSuccess { get; set; } = true;
		public object? Data { get; set; } = null;
    }
}