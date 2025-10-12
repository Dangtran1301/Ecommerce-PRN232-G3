using CatalogService.API.Entities;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;

namespace CatalogService.API.Repositories.Interfaces
{
    public interface ICategoryRepository : IRepository<Category, Guid>
    {
        Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    }
}
