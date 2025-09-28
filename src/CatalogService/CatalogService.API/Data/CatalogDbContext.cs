using Microsoft.EntityFrameworkCore;
using CatalogService.Entities;
using SharedKernel.Infrastructure.Data;
namespace CatalogService.Data
{
    public class CatalogDbContext : BaseDbContext
    {
        protected CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    // Category
        //    modelBuilder.Entity<Category>()
        //        .HasIndex(c => c.CategoryName)
        //        .IsUnique();
        //    // Brand
        //    modelBuilder.Entity<Brand>()
        //        .HasIndex(b => b.BrandName)
        //        .IsUnique();
        //    // Product
        //    modelBuilder.Entity<Product>()
        //        .HasKey(p => p.ProductId);
        //    modelBuilder.Entity<Product>()
        //        .HasOne(p => p.Category)
        //        .WithMany(c => c.Products)
        //        .HasForeignKey(p => p.CategoryId)
        //        .OnDelete(DeleteBehavior.Cascade);
        //    modelBuilder.Entity<Product>()
        //        .HasOne(p => p.Brand)
        //        .WithMany(b => b.Products)
        //        .HasForeignKey(p => p.BrandId)
        //        .OnDelete(DeleteBehavior.Cascade);
        //    // Outbox
        //    modelBuilder.Entity<OutboxMessage>()
        //        .HasKey(o => o.Id);
        //}
    }
}
