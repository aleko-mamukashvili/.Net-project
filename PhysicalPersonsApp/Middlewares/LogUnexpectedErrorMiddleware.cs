namespace PhysicalPersonsApp.Middlewares;

public class LogUnexpectedErrorMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LogUnexpectedErrorMiddleware> _logger;

    public LogUnexpectedErrorMiddleware(RequestDelegate next, ILogger<LogUnexpectedErrorMiddleware> logger)
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
            _logger.LogError(ex, "An unexpected error occurred while processing the request.");

            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            var errorResponse = new { message = "An internal server error." };
            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}