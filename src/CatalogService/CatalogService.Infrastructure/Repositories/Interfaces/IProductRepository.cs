using CatalogService.Entities;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;

namespace CatalogService.Infrastructure.Repositories.Interfaces
{
    public interface IProductRepository : IRepository<Product, Guid>
    {
        Task<Product?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

        IQueryable<Product> GetQueryable();
    }
}