using SharedKernel.Domain.Common.Events.Interface;

namespace SharedKernel.Domain.Common.Events;

public class DomainEvent : IDomainEvent
{
    public Guid AggregateId { get; private set; }
    public DateTime OccurredOn { get; private set; }= DateTime.Now;

    protected DomainEvent(Guid aggregateId)
    {
        AggregateId = aggregateId;
    }
}