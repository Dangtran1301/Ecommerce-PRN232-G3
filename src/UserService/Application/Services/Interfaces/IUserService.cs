using SharedKernel.Domain.Common.Results;
using UserService.Application.DTOs;

namespace UserService.Application.Services.Interfaces;

public interface IUserService
{
    Task<Result<UserDto>> GetByIdAsync(Guid id);

    Task<Result<IReadOnlyList<UserDto>>> GetAllAsync();

    Task<Result<UserDto>> CreateAsync(CreateUserRequest request);

    Task<Result<UserDto>> UpdateAsync(Guid id, UpdateUserRequest request);

    Task<Result> DeleteAsync(Guid id);
}