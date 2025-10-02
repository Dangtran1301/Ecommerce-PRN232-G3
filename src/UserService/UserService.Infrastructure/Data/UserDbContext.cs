using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain.Common.Events;
using SharedKernel.Infrastructure.Data;
using System.Reflection;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Data;

public class UserDbContext(DbContextOptions<UserDbContext> options) : BaseDbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


        modelBuilder.Entity<OutboxMessage>(entity =>
        {
            entity.ToTable("Outbox_User");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.EventType).IsRequired();
            entity.Property(x => x.Payload).IsRequired();

            entity.Property(x => x.Status)
                .HasConversion<string>()
                .HasDefaultValue(OutboxStatus.Pending);
        });
    }
}