using AuthService.API.DTOs;
using SharedKernel.Domain.Common.Results;

namespace AuthService.API.Interfaces;

public interface IUserInternalClient
{
    Task<Result<UserServiceUserDto?>> ValidateUserAsync(LoginRequestDto payload, CancellationToken cancellationToken = default);

    Task<Result<UserServiceUserDto?>> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
}