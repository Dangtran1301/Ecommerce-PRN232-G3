using CatalogService.API.Entities;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;

namespace CatalogService.API.Repositories.Interfaces

{
    public interface IBrandRepository : IRepository<Brand, Guid>
    {
        Task<Brand?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    }
}
