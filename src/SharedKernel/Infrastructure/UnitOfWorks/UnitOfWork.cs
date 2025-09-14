using SharedKernel.Infrastructure.Data.Interfaces;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;

namespace SharedKernel.Infrastructure.UnitOfWorks;

public class UnitOfWork(IDbContext dbContext) : IUnitOfWork
{
    private readonly IDbContext _dbContext = dbContext;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}