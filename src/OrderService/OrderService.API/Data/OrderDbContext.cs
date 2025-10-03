using Microsoft.EntityFrameworkCore;
using OrderService.API.Models;
using SharedKernel.Infrastructure.Data;

namespace OrderService.API.Data
{
    public class OrderDbContext : BaseDbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // gọi base để áp dụng soft delete filter + logic chung
            base.OnModelCreating(modelBuilder);

            // Config Order
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.Id);

                entity.HasMany(o => o.Items)
                      .WithOne(i => i.Order)
                      .HasForeignKey(i => i.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(o => o.TotalAmount)
                      .HasColumnType("decimal(18,2)");
            });

            // Config OrderItem
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(oi => oi.Id);

                entity.Property(oi => oi.Price)
                      .HasColumnType("decimal(18,2)");
            });
        }
    }
}
