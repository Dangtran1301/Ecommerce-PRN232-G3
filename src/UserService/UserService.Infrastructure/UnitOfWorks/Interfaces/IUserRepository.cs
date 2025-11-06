using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.UnitOfWorks.Interfaces;

public interface IUserRepository : IRepository<UserProfile, Guid>
{
}