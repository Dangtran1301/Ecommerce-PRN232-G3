
using CatalogService.Application.DTOs.Brands;
using CatalogService.Application.DTOs.Categories;
using CatalogService.Application.DTOs.ProductAttributes;
using CatalogService.Application.DTOs.Products;
using CatalogService.Application.DTOs.ProductVariants;
using CatalogService.Application.DTOs.Stocks;
using CatalogService.Application.Services;
using CatalogService.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;
using SharedKernel.Infrastructure.UnitOfWorks.Repositories;

namespace CatalogService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // AutoMapper
        services.AddAutoMapper(typeof(BrandProfile).Assembly);
        services.AddAutoMapper(typeof(CategoryProfile).Assembly);
        services.AddAutoMapper(typeof(ProductAttributeProfile).Assembly);
        services.AddAutoMapper(typeof(ProductProfile).Assembly);
        services.AddAutoMapper(typeof(ProductVariantProfile).Assembly);
        services.AddAutoMapper(typeof(StockProfile).Assembly);

        // Application Services
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IBrandService, BrandService>();
        services.AddScoped<IProductService, ProductAppService>();
        services.AddScoped<IProductVariantService, ProductVariantService>();
        services.AddScoped<IProductAttributeService, ProductAttributeService>();
        services.AddScoped<IStockService, StockService>();
        services.AddScoped(typeof(ISpecificationRepository<>), typeof(SpecificationRepository<>));

        services.AddScoped(typeof(IDynamicRepository<>), typeof(DynamicRepository<>));

        return services;
    }
}