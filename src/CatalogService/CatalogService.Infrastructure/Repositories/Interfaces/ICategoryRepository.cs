using CatalogService.Domain.Entities;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;

namespace CatalogService.Infrastructure.Repositories.Interfaces
{
    public interface ICategoryRepository : IRepository<Category, Guid>
    {
        Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
        IQueryable<Category> GetQueryable();
    }
}
