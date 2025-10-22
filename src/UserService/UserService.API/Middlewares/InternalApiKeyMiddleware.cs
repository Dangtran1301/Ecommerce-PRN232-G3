namespace UserService.API.Middlewares;

public class InternalApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _headerName = "X-Internal-Key";
    private readonly string _expectedKey;

    public InternalApiKeyMiddleware(RequestDelegate next, IConfiguration config)
    {
        _next = next;
        _expectedKey = config["InternalApi:Key"] ?? string.Empty;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/api/internal"))
        {
            if (!context.Request.Headers.TryGetValue(_headerName, out var key) ||
                string.IsNullOrEmpty(_expectedKey) ||
                !string.Equals(key, _expectedKey))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized internal request");
                return;
            }
        }

        await _next(context);
    }
}