namespace SharedKernel.Infrastructure.UnitOfWorks.Interfaces;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}