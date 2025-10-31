using System.Security.Claims;

namespace AuthService.API.Interfaces;

public interface IClaimsService
{
    ClaimsPrincipal? GetPrincipalFromToken(string token);
    string? GetUserId(string token);
    string? GetEmail(string token);
    string? GetFullName(string token);
    string? GetRole(string token);

    ClaimsPrincipal? CurrentUser { get; }
    string? CurrentUserId { get; }
    string? CurrentUserEmail { get; }
    string? CurrentUserFullName { get; }
    string? CurrentUserRole { get; }
}