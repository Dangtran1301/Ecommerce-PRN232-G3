using Asp.Versioning;
using AuthService.API.DTOs;
using AuthService.API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Application.Extensions;

namespace AuthService.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto dto, CancellationToken cancellationToken)
    {
        return (await authService.LoginAsync(dto, cancellationToken)).ToActionResult();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestDto dto, CancellationToken cancellationToken)
    {
        return (await authService.RegisterAsync(dto, cancellationToken)).ToActionResult();
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenRequestDto refreshTokenRequestDto)
    {
        return (await authService.RefreshAsync(refreshTokenRequestDto)).ToActionResult();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(RefreshTokenRequestDto refreshTokenRequestDto)
    {
        return (await authService.LogoutAsync(refreshTokenRequestDto)).ToActionResult();
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest forgotPasswordRequest, CancellationToken cancellation)
    {
        return (await authService.ForgotPasswordAsync(forgotPasswordRequest, cancellation)).ToActionResult();
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        return (await authService.ResetPasswordAsync(request, cancellationToken: cancellationToken)).ToActionResult();
    }
}