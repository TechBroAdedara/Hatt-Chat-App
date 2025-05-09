﻿using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Hatt.Infrastructure
{
    public class GlobalExceptionHandler (ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {

        var statusCode = exception switch
            {
                ArgumentNullException => StatusCodes.Status400BadRequest,
                KeyNotFoundException => StatusCodes.Status404NotFound,
                InvalidOperationException => StatusCodes.Status400BadRequest,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                ArgumentOutOfRangeException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };
            logger.LogError($"{DateTime.UtcNow} An error {statusCode} occured : {exception.Message}");

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = GetTitleForException(exception),
                Detail = statusCode != 500 ? exception.Message : "Something went wrong. Contact Admin.",
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
                UnauthorizedAccessException => "Not Authorized",
                ArgumentOutOfRangeException => "Invalid motive",
                _ => "An error occured"
            };
        }
    }
}
