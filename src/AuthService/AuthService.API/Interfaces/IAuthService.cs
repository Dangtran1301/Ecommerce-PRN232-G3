using AuthService.API.DTOs;
using SharedKernel.Domain.Common.Results;

namespace AuthService.API.Interfaces;

public interface IAuthService
{
    Task<Result<LoginResponse>> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken = default);

    Task<Result<LoginResponse>> RefreshAsync(RefreshTokenRequest refreshTokenRequest, CancellationToken cancellationToken = default);

    Task<Result> LogoutAsync(RefreshTokenRequest refreshTokenRequest, CancellationToken cancellationToken = default);

    Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);

    Task<Result> ForgotPasswordAsync(ForgotPasswordRequest forgotPasswordRequest, CancellationToken cancellationToken = default);

    Task<Result> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken = default);
}