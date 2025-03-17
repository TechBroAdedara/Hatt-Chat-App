using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Hatt.Infrastructure
{
    public class GlobalExceptionHandler (ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger = logger;

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {

        var statusCode = exception switch
            {
                ArgumentNullException => StatusCodes.Status400BadRequest,
                KeyNotFoundException => StatusCodes.Status404NotFound,
                InvalidOperationException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };
            logger.LogError($"{DateTime.UtcNow} An error {statusCode} occured : {exception.Message}");

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = GetTitleForException(exception),
                Detail = exception.Message,
            };

            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/problem+json";
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }

        private static string GetTitleForException(Exception exception) 
        { 
            return exception switch
            {
                ArgumentNullException => "Invalid Argument",
                KeyNotFoundException => "Resource Not Found",
                InvalidOperationException => "Invalid Operation",
                _ => "An error occured"
            };
        }
    }
}
