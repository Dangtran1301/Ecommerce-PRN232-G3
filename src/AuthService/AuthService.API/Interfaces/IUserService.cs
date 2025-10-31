using AuthService.API.DTOs;
using SharedKernel.Domain.Common.Results;

namespace AuthService.API.Interfaces;

public interface IUserService
{
    Task<Result<AuthUserResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<AuthUserResponse>>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Result> CreateAsync(RegisterRequest request, CancellationToken cancellationToken = default);

    Task<Result> RemoteRoleAsync(Guid id, RemoteAuthUserRequest request, CancellationToken cancellationToken = default);

    Task<Result> UpdateStatusUserAsync(Guid id, UpdateStatusAuthUserRequest request, CancellationToken cancellationToken = default);


    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<AuthUserResponse>>> FilterBySpecification(UserFilterRequest filter, CancellationToken cancellationToken = default);
}