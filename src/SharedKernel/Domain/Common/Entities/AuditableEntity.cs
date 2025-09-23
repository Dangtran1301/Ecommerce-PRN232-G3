using SharedKernel.Domain.Common.Entities.Interface;

namespace SharedKernel.Domain.Common.Entities;

public abstract class AuditableEntity<TKey> : Entity<TKey>, IAuditable
{
    public DateTime CreatedAt { get; protected set; }
    public string? CreatedBy { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }
    public string? UpdatedBy { get; protected set; }

    public void SetCreated(string? user = null)
    {
        CreatedAt = DateTime.UtcNow;
        CreatedBy = user;
    }

    public void SetUpdated(string? user = null)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = user;
    }
}