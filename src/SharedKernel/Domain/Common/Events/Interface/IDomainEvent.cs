namespace SharedKernel.Domain.Common.Events.Interface;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}