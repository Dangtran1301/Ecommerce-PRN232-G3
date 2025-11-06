namespace SharedKernel.Domain.Common.Entities.Interface;

public interface IEntity<TKey>
{
    TKey Id { get; }
}