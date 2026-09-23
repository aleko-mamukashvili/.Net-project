namespace PhysicalPersonsApp.Middlewares;
public class LocalizationMiddleware
{
    private readonly RequestDelegate _next;

    public LocalizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Request.Headers["Accept-Language"] = "ka-GE";

        var lang = context.Request.Query["lang"];
        if (!string.IsNullOrEmpty(lang))
        {
            context.Request.Headers["Accept-Language"] = lang;
        }

        await _next(context);
    }
}
