using UserService.API.Entities;

namespace UserService.API.Data.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);

    Task<User?> GetByUserNameAsync(string username);

    Task AddAsync(User user);

    Task UpdateAsync(User user);
}