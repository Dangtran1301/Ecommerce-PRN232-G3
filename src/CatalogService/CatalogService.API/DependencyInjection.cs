using Asp.Versioning;
using CatalogService.API.DTOs;
using CatalogService.API.Mappings;
using CatalogService.API.Repositories;
using CatalogService.API.Repositories.Interfaces;
using CatalogService.API.Services;
using CatalogService.API.Services.Interfaces;
using CatalogService.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Common;
using SharedKernel.Domain.Common.Results;
using SharedKernel.Infrastructure.Data.Interfaces;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;
using SharedKernel.Infrastructure.UnitOfWorks.Repositories;

namespace CatalogService;

public static class DependencyInjection
{
    public static IServiceCollection AddCatalogServices(this IServiceCollection services, IConfiguration configuration)
    {
        // ========== Infrastructure ==========
        services.AddDbContext<CatalogDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("CatalogDb"));
        });

        services.AddScoped<IDbContext, CatalogDbContext>();
        services.AddScoped<CatalogDbContext>();

        services.AddScoped<IBrandRepository, BrandRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped(typeof(IRepository<,>), typeof(EfRepository<,>));

        // ========== Application ==========
        services.AddAutoMapper(typeof(BrandProfile).Assembly);
        services.AddScoped<IBrandService, BrandService>();
        services.AddScoped<ICategoryService, CategoryService>();

        services.AddScoped(typeof(ISpecificationRepository<>), typeof(SpecificationRepository<>));
        services.AddScoped(typeof(IDynamicRepository<>), typeof(DynamicRepository<>));

        // ========== API Presentation ==========
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
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                var error = Error.Validation("One or more validation errors occurred", errors);
                var response = new ApiResponse(false, error);

                return new BadRequestObjectResult(response);
            };
        });

        return services;
    }
}
