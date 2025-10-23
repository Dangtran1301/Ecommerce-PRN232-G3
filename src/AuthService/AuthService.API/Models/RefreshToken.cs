using SharedKernel.Domain.Common.Entities;

namespace AuthService.API.Models;

public class RefreshToken : Entity<int>
{
    public Guid UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public DateTime ExpiryDate { get; set; }
    public bool IsRevoked { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string CreatedByIp { get; set; } = string.Empty;

    public static RefreshToken Create(Guid userId, string token, string userName, DateTime expiry)
        => new RefreshToken
        {
            UserId = userId,
            Token = token,
            UserName = userName,
            ExpiryDate = expiry,
        };

    public void Revoke() => IsRevoked = true;

    public bool IsExpired() => DateTime.UtcNow >= ExpiryDate;
}