using Microsoft.EntityFrameworkCore;
using SharedKernel.Infrastructure.Data.Interfaces;
using SharedKernel.Infrastructure.UnitOfWorks.Repositories;
using UserService.Domain.Entities;
using UserService.Infrastructure.UnitOfWorks.Interfaces;

namespace UserService.Infrastructure.UnitOfWorks.Repositories;

public class UserRepository(IDbContext dbContext) : EfRepository<User, Guid>(dbContext), IUserRepository
{
    private readonly DbSet<User> _users = dbContext.Set<User>();

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }
}