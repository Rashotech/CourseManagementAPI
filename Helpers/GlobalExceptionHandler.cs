using System.Diagnostics;
using CourseManagement.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagement.Helpers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

            _logger.LogError(
                exception, "Exception occurred: {Message}, TraceId: {TraceId}", exception.Message, traceId);

            var (status, title) = MapException(exception);

            var problemDetails = new ProblemDetails
            {
                Status = status,
                Title = title,
                Detail = exception.Message,
                Extensions = new Dictionary<string, object?>
                {
                    { "traceId",  traceId },
                    { "errors",  GetErrorMessages(exception) }
                }
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response
                .WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
        
        private static IEnumerable<string> GetErrorMessages(Exception exception)
        {
            var messages = new List<string>();

            // Traverse the exception hierarchy to gather error messages
            while (exception != null)
            {
                messages.Add(exception.Message);
                exception = exception.InnerException;
            }

            return messages;
        }


        private static (int status, string Title) MapException(Exception exception)
        {
            return exception switch
            {
                ArgumentOutOfRangeException => (StatusCodes.Status400BadRequest, "Invalid argument provided."),
                KeyNotFoundException _ => (StatusCodes.Status404NotFound, "Resource not found."),
                BadRequestException _ => (StatusCodes.Status400BadRequest, "Bad Request."),
                NotFoundException _ => (StatusCodes.Status404NotFound, "Resource not found."),
                _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
            };
        }

    }
}