using AuthService.API.DTOs;
using AuthService.API.Errors;
using AuthService.API.Interfaces;
using AuthService.API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain.Common.Results;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;

namespace AuthService.API.Services;

public class AuthService(
    IRepository<RefreshToken, int> tokenRepository,
    IRepository<User, Guid> userRepository,
    IUserInternalClient userInternalClient,
    ITokenService tokenService,
    IMapper mapper,
    IHttpContextAccessor httpContextAccessor
    ) : IAuthService
{
    public async Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequestDto, CancellationToken cancellationToken = default)
    {
        var userQuery = userRepository.AsQueryable();

        var user = await userQuery.FirstOrDefaultAsync(x =>
                x.Email.Equals(loginRequestDto.Username) ||
                x.UserName.Equals(loginRequestDto.Username),
            cancellationToken);

        if (user is null || !BCrypt.Net.BCrypt.Verify(loginRequestDto.Password, user.PasswordHash))
            return Result.Fail<LoginResponseDto>(AuthErrors.InvalidCredentials());

        var userProfile = await userInternalClient.GetUserByIdAsync(user.Id, cancellationToken);

        if (!userProfile.IsSuccess || userProfile.Value is null)
            return AuthErrors.InvalidCredentials();

        var (accessToken, accessTokenExpiresAt) = tokenService.GenerateAccessToken(user, userProfile.Value);
        var (refreshToken, refreshExpiresAt) = tokenService.GenerateRefreshToken();

        var refreshEntity = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            ExpiryDate = refreshExpiresAt,
            IsRevoked = false,
            CreatedByIp = GetClientIp()
        };
        await tokenRepository.AddAsync(refreshEntity, cancellationToken);
        var authUser = mapper.Map<AuthUserDto>(user);
        authUser.FullName = userProfile.Value.FullName;
        var response = new LoginResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = accessTokenExpiresAt,
            User = authUser
        };

        return Result.Ok(response);
    }

    public async Task<Result<LoginResponseDto>> RefreshAsync(RefreshTokenRequestDto refreshTokenRequestDto, CancellationToken cancellationToken = default)
    {
        var existing = await tokenRepository.AsQueryable().FirstOrDefaultAsync(x =>
            x.Token == refreshTokenRequestDto.RefreshToken, cancellationToken);

        if (existing is null)
            return AuthErrors.InvalidRefreshToken;

        if (existing.IsRevoked)
            return AuthErrors.RefreshTokenRevoked;

        if (existing.ExpiryDate < DateTime.UtcNow)
            return AuthErrors.RefreshTokenExpired;

        var user = await userRepository.GetByIdAsync(existing.UserId, cancellationToken);

        if (user is null) 
            return AuthErrors.UserNotFound;

        var userProfileResponse = await userInternalClient.GetUserByIdAsync(existing.UserId, cancellationToken);

        if (!userProfileResponse.IsSuccess || userProfileResponse.Value is null)
            return AuthErrors.UserNotFound;

        var (accessToken, expiresAt) = tokenService.GenerateAccessToken(user, userProfileResponse.Value);
        var (newRefreshToken, newRefreshExpiresAt) = tokenService.GenerateRefreshToken();

        existing.IsRevoked = true;
        await tokenRepository.Update(existing, cancellationToken);

        var newEntity = new RefreshToken
        {
            UserId = user.Id,
            Token = newRefreshToken,
            ExpiryDate = newRefreshExpiresAt,
            IsRevoked = false,
            CreatedByIp = GetClientIp()
        };
        await tokenRepository.AddAsync(newEntity, cancellationToken);

        var response = mapper.Map<LoginResponseDto>(user);
        response.AccessToken = accessToken;
        response.RefreshToken = newRefreshToken;
        response.ExpiresAt = expiresAt;

        return Result.Ok(response);
    }

    public async Task<Result> LogoutAsync(RefreshTokenRequestDto refreshTokenRequestDto, CancellationToken cancellationToken = default)
    {
        var token = await tokenRepository.AsQueryable().AsNoTracking().FirstOrDefaultAsync(x =>
            x.Token == refreshTokenRequestDto.RefreshToken, cancellationToken);

        if (token is null)
            return AuthErrors.InvalidRefreshToken;

        if (token.IsRevoked)
            return AuthErrors.TokenAlreadyRevoked;

        token.IsRevoked = true;
        await tokenRepository.Update(token, cancellationToken);

        return Result.Ok();
    }

    public async Task<Result<UserServiceUserDto>> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default)
    {
        var exists = await userRepository.AnyAsync(u => u.UserName == request.UserName || u.Email == request.Email, cancellationToken);
        if (exists)
            return AuthErrors.AlreadyExisted;

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User(request.UserName, request.Email, passwordHash);
        await userRepository.AddAsync(user, cancellationToken);

        var profileResult = await userInternalClient.CreateUserProfileAsync(new CreateUserProfileInternalRequest(
            user.Id,
            request.FullName,
            request.PhoneNumber,
            request.Gender,
            request.DayOfBirth,
            request.Address,
            request.Avatar), cancellationToken);

        return !profileResult.IsSuccess
            ? Result.Fail<UserServiceUserDto>(Error.Failure("Failed to create user profile"))
            : Result.Ok(profileResult.Value!);
    }

    private string GetClientIp()
    {
        var ip = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
        return string.IsNullOrEmpty(ip) ? "unknown" : ip;
    }
}