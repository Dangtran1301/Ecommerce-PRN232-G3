using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Application.Common;
using SharedKernel.Domain.Common.Results;

namespace CatalogService.API;

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
        services.Configure<ApiBehaviorOptions>(static options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(ms => ms.Value?.Errors.Count > 0)
                    .SelectMany(kvp => kvp.Value!.Errors.Select(e => new
                    {
                        Field = kvp.Key,
                        Error = e.ErrorMessage
                    }))
                    .ToList();

                var error = Error.Validation(
                    "One or more validation errors occurred",
                    errors
                );

                var response = new ApiResponse { Error = error };

                return new BadRequestObjectResult(response);
            };
        });
        return services;
    }
}