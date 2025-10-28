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
    IRepository<RefreshToken, int> repository,
    IUserInternalClient userInternalClient,
    ITokenService tokenService,
    IMapper mapper,
    IHttpContextAccessor httpContextAccessor
    ) : IAuthService
{
    public async Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequestDto, CancellationToken cancellationToken = default)
    {
        var userResult = await userInternalClient.ValidateUserAsync(loginRequestDto, cancellationToken);

        if (!userResult.IsSuccess)
            return Result.Fail<LoginResponseDto>(AuthErrors.InvalidCredentials(
                userResult.Error.Details?.ToString()));

        if (userResult.Value is null)
            return Result.Fail<LoginResponseDto>(AuthErrors.NullUser);

        var user = userResult.Value;

        var (accessToken, accessTokenExpiresAt) = tokenService.GenerateAccessToken(user);
        var (refreshToken, refreshExpiresAt) = tokenService.GenerateRefreshToken();

        var refreshEntity = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            ExpiryDate = refreshExpiresAt,
            IsRevoked = false,
            CreatedByIp = GetClientIp()
        };
        await repository.AddAsync(refreshEntity, cancellationToken);

        var response = mapper.Map<LoginResponseDto>(user);
        response.AccessToken = accessToken;
        response.RefreshToken = refreshToken;
        response.ExpiresAt = accessTokenExpiresAt;

        return Result.Ok(response);
    }

    public async Task<Result<LoginResponseDto>> RefreshAsync(RefreshTokenRequestDto refreshTokenRequestDto, CancellationToken cancellationToken = default)
    {
        var existing = await repository.AsQueryable().FirstOrDefaultAsync(x =>
            x.Token == refreshTokenRequestDto.RefreshToken, cancellationToken);

        if (existing is null)
            return AuthErrors.InvalidRefreshToken;

        if (existing.IsRevoked)
            return AuthErrors.RefreshTokenRevoked;

        if (existing.ExpiryDate < DateTime.UtcNow)
            return AuthErrors.RefreshTokenExpired;

        var userResult = await userInternalClient.GetUserByIdAsync(existing.UserId, cancellationToken);
        if (!userResult.IsSuccess || userResult.Value is null)
            return AuthErrors.UserNotFound;

        var user = userResult.Value;

        var (accessToken, expiresAt) = tokenService.GenerateAccessToken(user);
        var (newRefreshToken, newRefreshExpiresAt) = tokenService.GenerateRefreshToken();

        existing.IsRevoked = true;
        await repository.Update(existing, cancellationToken);

        var newEntity = new RefreshToken
        {
            UserId = user.Id,
            Token = newRefreshToken,
            ExpiryDate = newRefreshExpiresAt,
            IsRevoked = false,
            CreatedByIp = GetClientIp()
        };
        await repository.AddAsync(newEntity, cancellationToken);

        var response = mapper.Map<LoginResponseDto>(user);
        response.AccessToken = accessToken;
        response.RefreshToken = newRefreshToken;
        response.ExpiresAt = expiresAt;

        return Result.Ok(response);
    }

    public async Task<Result> LogoutAsync(RefreshTokenRequestDto refreshTokenRequestDto, CancellationToken cancellationToken = default)
    {
        var token = await repository.AsQueryable().AsNoTracking().FirstOrDefaultAsync(x =>
            x.Token == refreshTokenRequestDto.RefreshToken, cancellationToken);

        if (token is null)
            return AuthErrors.InvalidRefreshToken;

        if (token.IsRevoked)
            return AuthErrors.TokenAlreadyRevoked;

        token.IsRevoked = true;
        await repository.Update(token, cancellationToken);

        return Result.Ok();
    }

    private string GetClientIp()
    {
        var ip = httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
        return string.IsNullOrEmpty(ip) ? "unknown" : ip;
    }
}