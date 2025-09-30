using Asp.Versioning;

namespace UserService.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiPresentation(this IServiceCollection services)
    {
        services.AddControllers();

        services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new QueryStringApiVersionReader("api-version"),
                    new HeaderApiVersionReader("X-API-Version"),
                    new UrlSegmentApiVersionReader(),
                    new MediaTypeApiVersionReader("api-version")
                );
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }
}