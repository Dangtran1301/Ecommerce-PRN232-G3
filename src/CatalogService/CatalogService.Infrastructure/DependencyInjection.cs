using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.Data.Interfaces;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;
using SharedKernel.Infrastructure.UnitOfWorks.Repositories;
using CatalogService.Infrastructure.Data;
using CatalogService.Infrastructure.Repositories.Interfaces;
using CatalogService.Infrastructure.Repositories;

namespace CatalogService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // DbContext
        services.AddDbContext<CatalogDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("CatalogDb"));
        });

        services.AddScoped<IDbContext, CatalogDbContext>();
        services.AddScoped<CatalogDbContext>();

        // Repository pattern
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IBrandRepository, BrandRepository>();
        services.AddScoped(typeof(IRepository<,>), typeof(EfRepository<,>));

        return services;
    }
}