using Microsoft.EntityFrameworkCore;
using SharedKernel.Infrastructure.Data.Interfaces;
using SharedKernel.Infrastructure.UnitOfWorks.Repositories;
using UserService.Domain.Entities;
using UserService.Infrastructure.UnitOfWorks.Interfaces;

namespace UserService.Infrastructure.UnitOfWorks.Repositories;

public class UserRepository(IDbContext dbContext) : EfRepository<UserProfile, Guid>(dbContext), IUserRepository
{
    private readonly DbSet<UserProfile> _users = dbContext.Set<UserProfile>();
}