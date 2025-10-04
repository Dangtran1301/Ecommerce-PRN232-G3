using Microsoft.EntityFrameworkCore;
using OrderService.API.Models;
using SharedKernel.Domain.Common.Events;
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
        public DbSet<OutboxMessage> OutboxMessages { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // gọi logic soft delete & audit

            // Order
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

            // OrderItem
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(oi => oi.Id);

                entity.Property(oi => oi.Price)
                      .HasColumnType("decimal(18,2)");
            });

            // OutboxMessage
            modelBuilder.Entity<OutboxMessage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.EventType).IsRequired().HasMaxLength(200);
                entity.Property(e => e.AggregateType).HasMaxLength(200);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Payload).IsRequired();
                entity.Property(e => e.Metadata).HasColumnType("nvarchar(max)");
            });
        }
    }
}
