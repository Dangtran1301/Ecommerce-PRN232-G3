using SharedKernel.Domain.Common.Entities.Interface;

namespace SharedKernel.Domain.Common.Entities;

public abstract class SoftDeleteEntity<TKey> : AuditableEntity<TKey>, ISoftDelete
{
    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    public string? DeletedBy { get; private set; }

    public void MarkAsDeleted(string? user = null)
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = user;
    }

    public void Restore()
    {
        IsDeleted = false;
        DeletedAt = null;
        DeletedBy = null;
    }
}