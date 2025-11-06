using CatalogService.Domain.Entities;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;

namespace CatalogService.Infrastructure.Repositories.Interfaces

{
    public interface IBrandRepository : IRepository<Brand, Guid>
    {
        Task<Brand?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

        IQueryable<Brand> GetQueryable();
    }
}