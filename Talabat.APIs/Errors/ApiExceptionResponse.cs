using System.Net;

namespace Talabat.APIs.Errors
{
    public class ApiExceptionResponse:ApiResponse
    {
        public string? Details { get; set; }
        public ApiExceptionResponse(int StatusCode,string? Message=null,string? details = null):base((int)HttpStatusCode.InternalServerError,Message)
        {
            Details = details;
        }
    }
}
