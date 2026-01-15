using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace HomeHub.Api.Infrastructure
{
    public sealed class GlobalExceptionHandler : IExceptionHandler
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
            // 1. Log the error securely (so you can debug it in Kibana/Seq/Console)
            _logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

            // 2. Create the standard ProblemDetails object (RFC 7807)
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An internal error occurred",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
                Detail = "Something went wrong. Please try again later."
            };

            // --- CHAPTER 8: Handle Validation Errors ---
            if (exception is ValidationException validationEx)
            {
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Title = "Validation Error";
                problemDetails.Detail = "One or more validation errors occurred.";

                // Add the specific field errors to the response
                problemDetails.Extensions["errors"] = validationEx.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );
            }

            // 3. Add the TraceId (Critical for connecting frontend errors to backend logs)
            problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

            // 4. Write the response
            httpContext.Response.StatusCode = problemDetails.Status.Value;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true; // Signals that the exception was handled successfully
        }
    }
}
