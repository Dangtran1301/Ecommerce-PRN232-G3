using SharedKernel.Application.Common;
using SharedKernel.Domain.Common.Results;
using UserService.Application.DTOs;
using UserService.Domain.Entities.Enums;

namespace UserService.Application.Services.Interfaces;

public interface IUserProfileService
{
    Task<Result<UserProfileDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<UserProfileDto>>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Result> CreateAsync(CreateUserProfileRequest request, CancellationToken cancellationToken = default);

    Task<Result> CreateFromAuthAsync(Guid userId, string fullName, string? phoneNumber = null,
        Gender gender = Gender.Unknown, DateTime? dayOfBirth = null,
        string? address = null, string? avatar = null, CancellationToken cancellationToken = default);

    Task<Result> UpdateAsync(Guid id, UpdateUserProfileRequest request, CancellationToken cancellationToken = default);

    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<UserProfileDto>>> FilterBySpecification(UserProfileFilterDto filter, CancellationToken cancellationToken = default);

    Task<Result<PagedResult<UserProfileDto>>> FilterByDynamic(DynamicQuery query, CancellationToken cancellationToken = default);

    Task<Result<PagedResult<UserProfileDto>>> FilterPaged(PagedRequest request, CancellationToken cancellationToken = default);
}