using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.UnitOfWorks.Interfaces;

public interface IUserRepository : IRepository<User, Guid>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
}