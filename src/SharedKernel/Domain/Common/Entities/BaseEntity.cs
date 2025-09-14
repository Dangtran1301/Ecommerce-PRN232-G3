using SharedKernel.Domain.Common.Entities.Interface;

namespace SharedKernel.Domain.Common.Entities;

public abstract class BaseEntity<TKey> : IEntity<TKey>, IAuditable, ISoftDelete
{
    public TKey Id { get; protected set; } = default!;
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public string? CreatedBy { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }
    public string? UpdatedBy { get; protected set; }
    public bool IsDeleted { get; protected set; }
    public DateTime? DeletedAt { get; protected set; }
    public string? DeletedBy { get; protected set; }

    protected BaseEntity () { }

    protected BaseEntity(TKey id) => Id = id;
    public void SetCreated(string? user = null)
    {
        CreatedAt= DateTime.UtcNow;
        CreatedBy= user;
    }

    public void SetUpdated(string? user = null)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = user;
    }

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