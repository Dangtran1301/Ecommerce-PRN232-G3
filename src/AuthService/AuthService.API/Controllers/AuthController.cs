using Asp.Versioning;
using AuthService.API.DTOs;
using AuthService.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Application.Extensions;

namespace AuthService.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest dto, CancellationToken cancellationToken)
    {
        return (await authService.LoginAsync(dto, cancellationToken)).ToActionResult();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest dto, CancellationToken cancellationToken)
    {
        return (await authService.RegisterAsync(dto, cancellationToken)).ToActionResult();
    }

    [HttpPost("refresh")]
    [Authorize]
    public async Task<IActionResult> Refresh(RefreshTokenRequest refreshTokenRequest)
    {
        return (await authService.RefreshAsync(refreshTokenRequest)).ToActionResult();
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout(RefreshTokenRequest refreshTokenRequest)
    {
        return (await authService.LogoutAsync(refreshTokenRequest)).ToActionResult();
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

    [HttpPut("{id:guid}/password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(Guid id, [FromBody] ChangePasswordRequest request)
    {
        var result = await authService.ChangePasswordAsync(id, request);
        return result.ToActionResult();
    }
}