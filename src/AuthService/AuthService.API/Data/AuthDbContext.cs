using AuthService.API.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain.Common.Events;
using SharedKernel.Infrastructure.Data;

namespace AuthService.API.Data;

public class AuthDbContext(DbContextOptions options) : BaseDbContext(options)
{
    public DbSet<RefreshToken> UserSessions { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(u => u.Id);

            entity.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(512);

            entity.Property(u => u.Role)
                .HasConversion<string>()
                .HasMaxLength(16)
                .IsRequired();

            entity.Property(u => u.AccountStatus)
                .HasConversion<string>()
                .HasMaxLength(16)
                .IsRequired();

            entity.Property(u => u.ResetToken)
                .HasMaxLength(256)
                .IsUnicode(false)
                .IsRequired(false);

            entity.Property(x => x.CreatedBy)
                .HasMaxLength(256);

            entity.Property(x => x.UpdatedBy)
                .HasMaxLength(256);
            entity.Property(u => u.ResetTokenExpiry)
                .IsRequired(false);

            entity.HasIndex(u => u.Email).IsUnique();
            entity.HasIndex(u => u.UserName).IsUnique();
            entity.HasIndex(u => u.ResetToken);
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("RefreshTokens");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.UserId).IsRequired();
            entity.Property(x => x.Token)
                .IsRequired()
                .HasMaxLength(512);
            entity.Property(x => x.ExpiryDate).IsRequired();
            entity.Property(x => x.IsRevoked).IsRequired().HasDefaultValue(false);
            entity.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("GETUTCDATE()");
            entity.Property(x => x.CreatedByIp).HasMaxLength(45);

            entity.HasIndex(x => x.Token).IsUnique();
            entity.HasIndex(x => x.UserId);
        });

        modelBuilder.Entity<OutboxMessage>(entity =>
        {
            entity.ToTable("Outbox_Auth");
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
    public static void AddDbContextService(this IServiceCollection collection, ConfigurationManager configurationManager)
    {
        collection.AddDbContext<AuthDbContext>(opts =>
        {
            opts.UseSqlServer(configurationManager.GetConnectionString("AuthDb"));
        });
    }
}