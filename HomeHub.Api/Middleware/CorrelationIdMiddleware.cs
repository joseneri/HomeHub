using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HomeHub.Api.Infrastructure;

public sealed class CorrelationIdMiddleware
{
    private const string HeaderName = "X-Correlation-Id";
    private readonly RequestDelegate _next;
    private readonly ILogger<CorrelationIdMiddleware> _logger;

    public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // 1. Get or Generate ID
        string correlationId = GetCorrelationId(context);

        // 2. Sync with ASP.NET Core's native TraceIdentifier
        context.TraceIdentifier = correlationId;

        // 3. Return to Client (React needs to see this to log errors properly)
        context.Response.OnStarting(() =>
        {
            if (!context.Response.Headers.ContainsKey(HeaderName))
            {
                context.Response.Headers[HeaderName] = correlationId;
            }
            return Task.CompletedTask;
        });

        // 4. Create Logging Scope
        // Any log written inside this 'using' block will have { CorrelationId: "..." } automatically attached.
        using (_logger.BeginScope(new Dictionary<string, object>
        {
            ["CorrelationId"] = correlationId
        }))
        {
            await _next(context);
        }
    }

    private static string GetCorrelationId(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(HeaderName, out var incoming) &&
            !string.IsNullOrWhiteSpace(incoming))
        {
            return incoming.ToString();
        }
        return Guid.NewGuid().ToString();
    }
}
