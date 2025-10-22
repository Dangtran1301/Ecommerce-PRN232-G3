using AuthService.API.DTOs;
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
            return Result.Fail<LoginResponseDto>(userResult.Error!);
        if (userResult.Value is null)
            return Result.Fail<LoginResponseDto>(Error.Validation("User is null"));

        var user = userResult.Value;

        var (accessToken, accessTokenExpiresAt) = tokenService.GenerateAccessToken(user);

        var (refreshToken, refreshExpiresAt) = tokenService.GenerateRefreshToken();

        var refreshEntity = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            ExpiryDate = refreshExpiresAt,
            IsRevoked = false,
            UserName = user.UserName,
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
            return Result.Fail<LoginResponseDto>(Error.Validation("Invalid refresh token"));

        if (existing.IsRevoked)
            return Result.Fail<LoginResponseDto>(Error.Validation("Refresh token has been revoked"));

        if (existing.ExpiryDate < DateTime.UtcNow)
            return Result.Fail<LoginResponseDto>(Error.Validation("Refresh token has expired"));

        // Lấy lại thông tin user từ internal client
        var userResult = await userInternalClient.GetUserByIdAsync(existing.UserId, cancellationToken);
        if (!userResult.IsSuccess || userResult.Value is null)
            return Result.Fail<LoginResponseDto>(Error.Validation("User not found"));

        var user = userResult.Value;

        // Tạo token mới
        var (accessToken, expiresAt) = tokenService.GenerateAccessToken(user);
        var (newRefreshToken, newRefreshExpiresAt) = tokenService.GenerateRefreshToken();

        // Thu hồi token cũ
        existing.IsRevoked = true;
        await repository.Update(existing, cancellationToken);

        // Lưu refresh token mới
        var newEntity = new RefreshToken
        {
            UserId = user.Id,
            Token = newRefreshToken,
            ExpiryDate = newRefreshExpiresAt,
            IsRevoked = false,
            UserName = user.UserName,
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
            return Result.Fail(Error.Validation("Invalid refresh token"));

        if (token.IsRevoked)
            return Result.Fail(Error.Validation("Token already revoked"));

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