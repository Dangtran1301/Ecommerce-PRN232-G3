using AuthService.API.DTOs;
using AuthService.API.Errors;
using AuthService.API.Interfaces;
using AuthService.API.Models;
using AutoMapper;
using SharedKernel.Domain.Common.Results;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;

namespace AuthService.API.Services;

public class UserService(
    IRepository<User, Guid> userRepository,
    ISpecificationRepository<User> userSpecificationRepository,
    IUserInternalClient userInternalClient,
    IClaimsService claimsService,
    IMapper mapper
    ) : IUserService
{
    public async Task<Result<AuthUserResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(id, cancellationToken);
        return user is null ? AuthErrors.UserNotFound : Result.Ok(mapper.Map<AuthUserResponse>(user));
    }

    public async Task<Result<IReadOnlyList<AuthUserResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var users = await userRepository.GetAllAsync(cancellationToken);
        return Result.Ok(mapper.Map<IReadOnlyList<AuthUserResponse>>(users));
    }

    public async Task<Result> CreateAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        if (await userRepository.AnyAsync(u => u.UserName == request.UserName, cancellationToken))
            return AuthErrors.UsernameAlreadyExisted;

        if (await userRepository.AnyAsync(u => u.Email == request.Email, cancellationToken))
            return AuthErrors.EmailAlreadyExisted;

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var newUser = new User(request.UserName, request.Email, passwordHash);
        var profileResult = await userInternalClient.CreateUserProfileAsync(new CreateUserProfileInternalRequest(
            newUser.Id,
            request.FullName,
            request.PhoneNumber,
            request.Gender,
            request.DayOfBirth,
            request.Address,
            request.Avatar), cancellationToken);

        if (!profileResult.IsSuccess)
            return Result.Fail<UserProfileResponse>(Error.Failure("Failed to create user profile"));

        await userRepository.AddAsync(newUser, cancellationToken);
        return Result.Ok(profileResult.Value!);
    }

    public async Task<Result> RemoteRoleAsync(Guid id, RemoteAuthUserRequest request, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(id, cancellationToken);
        if (user is null) return AuthErrors.UserNotFound;
        if (!Enum.TryParse(request.Role, out Role newRole)) return AuthErrors.BadRequest("Invalid role!");
        user.ChangeRole(newRole);
        await userRepository.Update(user, cancellationToken);
        return Result.Ok();
    }

    public async Task<Result> UpdateStatusUserAsync(Guid id, UpdateStatusAuthUserRequest request, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(id, cancellationToken);
        if (user is null)
            return AuthErrors.UserNotFound;
        if (!Enum.TryParse(request.NewStatus, out AccountStatus newStatus))
            return AuthErrors.BadRequest("Invalid status!");
        if (user.AccountStatus != newStatus)
        {
            switch (newStatus)
            {
                case AccountStatus.Active:
                    user.Activate();
                    break;

                case AccountStatus.Inactive:
                    user.Deactivate();
                    break;
            }
        }

        await userRepository.Update(user, cancellationToken);
        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (claimsService.CurrentUserRole != nameof(Role.Admin)) return Error.Forbidden("Only admins can delete users");

        var user = await userRepository.GetByIdAsync(id, cancellationToken);

        if (user is null)
            return AuthErrors.UserNotFound;

        if (user.Id.ToString().Equals(claimsService.CurrentUserId))
            return AuthErrors.InvalidCredentials("Can't not delete your own account");

        var response = await userInternalClient.DeleteUserProfileAsync(id, cancellationToken);
        if (!response.IsSuccess)
            return AuthErrors.BadRequest("Can't not delete user's profile");
        await userRepository.Remove(user, cancellationToken);
        return Result.Ok();
    }

    public async Task<Result<IReadOnlyList<AuthUserResponse>>> FilterBySpecification(UserFilterRequest filter, CancellationToken cancellationToken = default)
    {
        var spec = (UserSpecification)filter;
        var users = await userSpecificationRepository.ListAsync(spec, cancellationToken);
        var result = mapper.Map<IReadOnlyList<AuthUserResponse>>(users);

        return Result.Ok(result);
    }
}