using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(
            exception,
            "Unhandled exception occurred. TraceId: {TraceId}",
            httpContext.TraceIdentifier);

        var problemDetails = exception switch
        {

            KeyNotFoundException ex => new ProblemDetails
            {
                Title = "Resource Not Found",
                Detail = ex.Message,
                Status = StatusCodes.Status404NotFound,
                Type = "https://httpstatuses.com/404"
            },

            ValidationException ex => new ProblemDetails
            {
                Title = "Validation Error",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest,
                Type = "https://httpstatuses.com/400"
            },

            _ => new ProblemDetails
            {
                Title = "Internal Server Error",
                Detail = "An unexpected error occurred.",
                Status = StatusCodes.Status500InternalServerError,
                Type = "https://httpstatuses.com/500"
            }
        };

        problemDetails.Extensions["traceId"] =
            httpContext.TraceIdentifier;

        httpContext.Response.StatusCode =
            problemDetails.Status!.Value;

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);

        return true;
    }
}
