using AutoMapper;
using SharedKernel.Application.Common;
using SharedKernel.Domain.Common.Results;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;
using UserService.Application.DTOs;
using UserService.Application.Errors;
using UserService.Application.Services.Interfaces;
using UserService.Domain.Entities.Enums;
using UserService.Infrastructure.UnitOfWorks.Interfaces;
using UserProfile = UserService.Domain.Entities.UserProfile;

namespace UserService.Application.Services;

public class UserProfileService(
    IUserRepository repository,
    ISpecificationRepository<UserProfile> specificationRepository,
    IDynamicRepository<UserProfile> dynamicRepository,
    IMapper mapper
) : IUserProfileService
{
    public async Task<Result<UserProfileDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await repository.GetByIdAsync(id, cancellationToken);
        return user is null
            ? UserProfileErrors.NotFound(id)
            : Result.Ok(mapper.Map<UserProfileDto>(user));
    }

    public async Task<Result<IReadOnlyList<UserProfileDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var users = await repository.GetAllAsync(cancellationToken);
        return Result.Ok(mapper.Map<IReadOnlyList<UserProfileDto>>(users));
    }

    public async Task<Result> CreateAsync(CreateUserProfileRequest request, CancellationToken cancellationToken = default)
    {
        var userProfile = new UserProfile(
            request.UserId,
            request.FullName,
            request.PhoneNumber
        )
        {
            Gender = request.Gender,
            DayOfBirth = request.DayOfBirth,
            Address = request.Address,
            Avatar = request.Avatar
        };

        await repository.AddAsync(userProfile, cancellationToken);
        return Result.Ok();
    }

    public async Task<Result> CreateFromAuthAsync(Guid userId, string fullName, string? phoneNumber = null, Gender gender = Gender.Unknown,
        DateTime? dayOfBirth = null, string? address = null, string? avatar = null, CancellationToken cancellationToken = default)
    {
        var existing = await repository.AnyAsync(x => x.Id.Equals(userId), cancellationToken);
        if (existing)
            return UserProfileErrors.UserIdTaken(userId);

        var profile = new UserProfile(userId, fullName, phoneNumber)
        {
            Gender = gender,
            DayOfBirth = dayOfBirth,
            Address = address,
            Avatar = avatar
        };

        await repository.AddAsync(profile, cancellationToken);
        return Result.Ok();
    }

    public async Task<Result> UpdateAsync(Guid id, UpdateUserProfileRequest request, CancellationToken cancellationToken = default)
    {
        var user = await repository.GetByIdAsync(id, cancellationToken);
        if (user is null)
            return UserProfileErrors.NotFound(id);

        if (!string.IsNullOrWhiteSpace(request.FullName))
            user.ChangeName(request.FullName);

        if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
            user.ChangePhone(request.PhoneNumber);

        if (!string.IsNullOrWhiteSpace(request.Avatar))
            user.ChangeAvatar(request.Avatar);

        if (!string.IsNullOrWhiteSpace(request.Address))
            user.ChangeAddress(request.Address);

        if (request.Gender.HasValue)
            user.Gender = request.Gender.Value;

        if (request.DayOfBirth.HasValue)
            user.DayOfBirth = request.DayOfBirth.Value;

        await repository.Update(user, cancellationToken);
        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await repository.GetByIdAsync(id, cancellationToken);
        if (user is null)
            return UserProfileErrors.NotFound(id);

        await repository.Remove(user, cancellationToken);
        return Result.Ok();
    }

    public async Task<Result<IReadOnlyList<UserProfileDto>>> FilterBySpecification(UserProfileFilterDto filter, CancellationToken cancellationToken = default)
    {
        var spec = new UserProfileFilterSpecification(filter);
        var users = await specificationRepository.ListAsync(spec, cancellationToken);
        return Result.Ok(mapper.Map<IReadOnlyList<UserProfileDto>>(users));
    }

    public async Task<Result<PagedResult<UserProfileDto>>> FilterByDynamic(DynamicQuery query, CancellationToken cancellationToken = default)
    {
        var result = await dynamicRepository.GetPagedAsync(query, cancellationToken);
        var dto = result.Map(mapper.Map<IReadOnlyList<UserProfileDto>>(result.Items));
        return Result.Ok(dto);
    }

    public async Task<Result<PagedResult<UserProfileDto>>> FilterPaged(PagedRequest request, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetPagedAsync(request, cancellationToken);
        var dto = result.Map(mapper.Map<IReadOnlyList<UserProfileDto>>(result.Items));
        return Result.Ok(dto);
    }
}