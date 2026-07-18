using System.Net;
using System.Text.Json;
using Api.Common.Records;

namespace Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await _HandleExceptionAsync(context, ex);
        }
    }

    private async Task _HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = _MapStatusCode(exception);

        _logger.LogError(exception, "Unhandled exception occurred while processing {Method} {Path}", context.Request.Method, context.Request.Path);

        var errorResponse = new ErrorResponse(exception.Message, statusCode);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }

    private static int _MapStatusCode(Exception exception)
    {
        var statusCode = exception switch
        {
            ArgumentException => (int)HttpStatusCode.BadRequest,
            KeyNotFoundException => (int)HttpStatusCode.NotFound,
            _ => (int)HttpStatusCode.InternalServerError
        };
        return statusCode;
    }
}
