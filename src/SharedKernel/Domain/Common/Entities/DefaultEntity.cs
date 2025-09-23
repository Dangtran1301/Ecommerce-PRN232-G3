using SharedKernel.Domain.Common.Entities.Interface;

namespace SharedKernel.Domain.Common.Entities;

public abstract class DefaultEntity<TKey> : Entity<TKey>
{
    public string Name { get; protected set; } = string.Empty;
    public string? Description { get; protected set; }
    public bool IsActive { get; protected set; } = true;

    public void Activate() => IsActive = true;

    public void Deactivate() => IsActive = false;
}

public abstract class Entity<TKey> : IEntity<TKey>
{
    public TKey Id { get; protected set; } = default!;
}