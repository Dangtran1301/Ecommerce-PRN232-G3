using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.FullName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.UserName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.Email)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.PasswordHash)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(u => u.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(u => u.Gender)
            .HasConversion<string>()
            .HasMaxLength(7);

        builder.Property(u => u.AccountStatus)
            .HasConversion<string>()
            .HasMaxLength(8);

        builder.Property(u => u.Role)
            .HasConversion<string>()
            .HasMaxLength(8);

        builder.Property(u => u.DayOfBirth);

        builder.Property(u => u.Address)
            .HasMaxLength(255);

        builder.Property(u => u.Avatar)
            .HasMaxLength(255);

        builder.HasMany(u => u.UserSessions)
            .WithOne(s => s.User)
            .HasForeignKey(s => s.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(u => u.Email).IsUnique();
        builder.HasIndex(u => u.UserName).IsUnique();
    }
}