using AuthService.API.DTOs;
using SharedKernel.Domain.Common.Results;

namespace AuthService.API.Interfaces;

public interface IAuthService
{
    Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequestDto, CancellationToken cancellationToken = default);

    Task<Result<LoginResponseDto>> RefreshAsync(RefreshTokenRequestDto refreshTokenRequestDto, CancellationToken cancellationToken = default);

    Task<Result> LogoutAsync(RefreshTokenRequestDto refreshTokenRequestDto, CancellationToken cancellationToken = default);

    Task<Result<UserServiceUserDto>> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default);
}