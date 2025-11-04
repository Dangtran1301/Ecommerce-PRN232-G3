using CatalogService.Domain.Entities;
using CatalogService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Infrastructure.Data.Interfaces;
using SharedKernel.Infrastructure.UnitOfWorks.Repositories;

namespace CatalogService.Infrastructure.Repositories
{
    public class CategoryRepository(IDbContext dbContext)
        : EfRepository<Category, Guid>(dbContext), ICategoryRepository
    {
        private readonly DbSet<Category> _categories = dbContext.Set<Category>();

        public async Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _categories.FirstOrDefaultAsync(c => c.CategoryName == name, cancellationToken);
        }

        public IQueryable<Category> GetQueryable()
        {
            return _categories.AsNoTracking();
        }
    }
}