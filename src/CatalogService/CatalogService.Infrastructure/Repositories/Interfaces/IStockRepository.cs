using CatalogService.Entities;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;

namespace CatalogService.Infrastructure.Repositories.Interfaces
{
    public interface IStockRepository : IRepository<Stock, Guid>
    {
        Task<Stock?> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
        IQueryable<Stock> GetQueryable();
    }
}
