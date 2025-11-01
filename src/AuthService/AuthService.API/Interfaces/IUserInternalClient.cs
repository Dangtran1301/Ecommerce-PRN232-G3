using AuthService.API.DTOs;
using SharedKernel.Domain.Common.Results;

namespace AuthService.API.Interfaces;

public interface IUserInternalClient
{
    Task<Result<UserProfileResponse?>> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result<UserProfileResponse?>> CreateUserProfileAsync(CreateUserProfileInternalRequest payload,
        CancellationToken cancellationToken = default);

    Task<Result> DeleteUserProfileAsync(Guid id, CancellationToken cancellationToken = default);
}