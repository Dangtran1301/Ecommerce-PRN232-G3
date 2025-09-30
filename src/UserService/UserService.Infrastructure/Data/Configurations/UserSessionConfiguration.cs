using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Data.Configurations;

public class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
{
    public void Configure(EntityTypeBuilder<UserSession> builder)
    {
        builder.ToTable("UserSessions");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.RefreshToken)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(s => s.ClientIp)
            .HasMaxLength(50);

        builder.Property(s => s.ExpiredTime)
            .IsRequired();

        builder.HasOne(s => s.User)
            .WithMany(u => u.UserSessions)
            .HasForeignKey(s => s.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}