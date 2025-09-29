using Microsoft.EntityFrameworkCore;
using CatalogService.Entities;
using SharedKernel.Infrastructure.Data;
namespace CatalogService.Data
{
        public class CatalogDbContext(DbContextOptions<CatalogDbContext> options) : BaseDbContext(options)
        {
            public DbSet<Category> Categories { get; set; }
            public DbSet<Brand> Brands { get; set; }
            public DbSet<Product> Products { get; set; }
            public DbSet<OutboxMessage> OutboxMessages { get; set; }
        }
    public static class DbContextExtensions
    {
        public static void AddDbContextService(this IServiceCollection services, IConfigurationManager configurationManager)
        {
            services.AddDbContext<CatalogDbContext>(optionsAction =>
            {
                optionsAction.UseSqlServer(configurationManager.GetConnectionString("CatalogDb"));
            });
        }
    }
}
