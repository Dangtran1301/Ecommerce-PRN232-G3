using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;
using SharedKernel.Infrastructure.UnitOfWorks.Repositories;
using CatalogService.Application.DTOs;
using CatalogService.Application.Services.Interfaces;

namespace CatalogService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // AutoMapper
        services.AddAutoMapper(typeof(BrandProfile).Assembly);
        services.AddAutoMapper(typeof(CategoryProfile).Assembly);

        // Application Services
        services.AddScoped<ICategoryService, Services.CategoryService>();
        services.AddScoped<IBrandService, Services.BrandService>();

        services.AddScoped(typeof(ISpecificationRepository<>), typeof(SpecificationRepository<>));

        services.AddScoped(typeof(IDynamicRepository<>), typeof(DynamicRepository<>));

        return services;
    }
}