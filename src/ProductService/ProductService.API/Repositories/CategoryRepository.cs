using CatalogService.API.Repositories.Interfaces;
using CatalogService.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Infrastructure.Data.Interfaces;
using SharedKernel.Infrastructure.UnitOfWorks.Repositories;

namespace CatalogService.API.Repositories
{
    public class CategoryRepository(IDbContext dbContext)
        : EfRepository<Category, Guid>(dbContext), ICategoryRepository
    {
        private readonly DbSet<Category> _categories = dbContext.Set<Category>();

        public async Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _categories.FirstOrDefaultAsync(c => c.CategoryName == name, cancellationToken);
        }
    }
}
