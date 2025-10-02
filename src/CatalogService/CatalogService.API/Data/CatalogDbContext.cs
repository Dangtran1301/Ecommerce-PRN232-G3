using Microsoft.EntityFrameworkCore;
using SharedKernel.Infrastructure.Data;
using System.Reflection;
using CatalogService.Entities;

namespace CatalogService.Data
{
    public class CatalogDbContext(DbContextOptions<CatalogDbContext> options) : BaseDbContext(options)
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OutboxMessage>(entity =>
            {
                entity.ToTable("Outbox_Catalog");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.EventType).IsRequired();
                entity.Property(x => x.Payload).IsRequired();

                entity.Property(x => x.Status)
                    .HasConversion<string>()
                    .HasDefaultValue(OutboxStatus.Pending);
            });
        }
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
