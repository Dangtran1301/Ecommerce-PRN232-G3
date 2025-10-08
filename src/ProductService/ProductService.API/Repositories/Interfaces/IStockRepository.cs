using CatalogService.Entities;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;

namespace CatalogService.API.Repositories.Interfaces
{
    public interface IStockRepository : IRepository<Stock, Guid>
    {
        Task<Stock?> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
        Task<bool> HasEnoughStockAsync(Guid productId, int requiredQuantity, CancellationToken cancellationToken = default);
    }
}
