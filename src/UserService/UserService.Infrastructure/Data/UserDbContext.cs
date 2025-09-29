using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.Data;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Data;

public class UserDbContext(DbContextOptions<UserDbContext> options) : BaseDbContext(options)
{
    public DbSet<User> Users { get; set; }
}

public static class DbContextExtensions
{
    public static void AddDbContextService(this IServiceCollection services, IConfigurationManager configurationManager)
    {
        services.AddDbContext<UserDbContext>(optionsAction =>
        {
            optionsAction.UseSqlServer(configurationManager.GetConnectionString("UserDb"));
        });
    }
}