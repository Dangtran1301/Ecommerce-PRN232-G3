using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.Data.Interfaces;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;
using SharedKernel.Infrastructure.UnitOfWorks.Repositories;
using UserService.Infrastructure.Data;
using UserService.Infrastructure.UnitOfWorks.Interfaces;
using UserService.Infrastructure.UnitOfWorks.Repositories;

namespace UserService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // DbContext
        services.AddDbContext<UserDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("UserDb"));
        });

        services.AddScoped<IDbContext, UserDbContext>();
        services.AddScoped<UserDbContext>();

        // Repository pattern
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped(typeof(IRepository<,>), typeof(EfRepository<,>));

        return services;
    }
}