using SharedKernel.Domain.Common.Entities;

namespace UserService.API.Entities;

public class UserSession : AuditableEntity<Guid>
{
    public string RefreshToken { get; set; } = string.Empty;
    public string? ClientIp { get; set; } = string.Empty;
    public DateTimeOffset ExpiredTime { get; set; } = default;
    public Guid AccountId { get; set; }
    public User User { get; set; } = default!;
}