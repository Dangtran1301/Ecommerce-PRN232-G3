using CatalogService.API.Entities;
using CatalogService.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Infrastructure.Data.Interfaces;
using SharedKernel.Infrastructure.UnitOfWorks.Repositories;

namespace CatalogService.API.Repositories
{
    public class BrandRepository(IDbContext dbContext)
        : EfRepository<Brand, Guid>(dbContext), IBrandRepository
    {
        private readonly DbSet<Brand> _brands = dbContext.Set<Brand>();

        public async Task<Brand?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _brands.FirstOrDefaultAsync(b => b.BrandName == name, cancellationToken);
        }
    }
}
