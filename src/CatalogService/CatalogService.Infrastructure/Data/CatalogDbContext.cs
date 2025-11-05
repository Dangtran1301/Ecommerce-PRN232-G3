using CatalogService.Domain.Entities;
using CatalogService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Domain.Common.Events;
using SharedKernel.Infrastructure.Data;
using System.Reflection;

namespace CatalogService.Infrastructure.Data
{
    public class CatalogDbContext(DbContextOptions<CatalogDbContext> options) : BaseDbContext(options)
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Stock> Stocks { get; set; } = null!;
        public DbSet<ProductVariant> ProductVariants { get; set; } = null!;
        public DbSet<ProductAttribute> ProductAttributes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);

            // 1-1: Product <-> Stock
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Stock)
                .WithOne(s => s.Product)
                .HasForeignKey<Stock>(s => s.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // 1-N: Brand <-> Product
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            // 1-N: Category <-> Product
            modelBuilder.Entity<Product>()
                .HasOne(static p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // 1-N: Product <-> ProductVariant
            modelBuilder.Entity<ProductVariant>()
                .HasOne(v => v.Product)
                .WithMany(p => p.Variants)
                .HasForeignKey(v => v.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // 1-N: Product <-> ProductAttribute
            modelBuilder.Entity<ProductAttribute>()
                .HasOne(a => a.Product)
                .WithMany(p => p.Attributes)
                .HasForeignKey(a => a.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OutboxMessage>(entity =>
            {
                entity.ToTable("Outbox_Catalog");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.EventType)
                      .IsRequired();

                entity.Property(x => x.Payload)
                      .IsRequired();

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