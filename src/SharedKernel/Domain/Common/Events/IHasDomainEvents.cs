namespace SharedKernel.Domain.Common.Events;

public interface IHasDomainEvents
{
    List<IIntegrationEvent> DomainEvents { get; }

    void ClearDomainEvents();
}