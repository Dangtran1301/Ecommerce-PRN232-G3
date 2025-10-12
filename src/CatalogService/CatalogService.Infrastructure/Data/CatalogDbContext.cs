using System.Reflection;
using CatalogService.Domain.Entities;
using SharedKernel.Domain.Common.Events;
using SharedKernel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
namespace CatalogService.Infrastructure.Data
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
