namespace SharedKernel.Domain.Common.Entities;

public abstract class DefaultEntity<TKey> : BaseEntity<TKey>
{
    public string Name { get; protected set; } = string.Empty;
    public string? Description { get; protected set; }
    public bool IsActive { get; protected set; } = true;

    protected DefaultEntity()
    { }

    protected DefaultEntity(TKey id, string name, string? description = null) : base(id)
    {
        Name = name;
        Description = description;
    }

    public void Activate() => IsActive = true;

    public void Deactivate() => IsActive = false;
}