using SharedKernel.Domain.Common.Events.Interface;

namespace SharedKernel.Domain.Common.Entities;

public abstract class AggregateRoot<TKey> : BaseEntity<TKey>
{
    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void RaiseEvent(IDomainEvent @event) => _domainEvents.Add(@event);
    public void ClearEvents() => _domainEvents.Clear();
}