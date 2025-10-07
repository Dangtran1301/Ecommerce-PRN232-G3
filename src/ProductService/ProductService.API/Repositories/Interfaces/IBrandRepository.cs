using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;
using CatalogService.Entities;
namespace CatalogService.API.Repositories.Interfaces

{
    public interface IBrandRepository : IRepository<Brand, Guid>
    {
        Task<Brand?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    }
}
