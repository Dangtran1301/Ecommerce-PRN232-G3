using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthService.API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.API.Services;

public class ClaimsService(IConfiguration config, IHttpContextAccessor httpContextAccessor)
    : IClaimsService
{
    #region From Raw Token

    public ClaimsPrincipal? GetPrincipalFromToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(config["Jwt:Key"]!);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = config["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = config["Jwt:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = true
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtToken ||
                !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return principal;
        }
        catch
        {
            return null;
        }
    }

    public string? GetUserId(string token)
        => GetPrincipalFromToken(token)?.FindFirstValue(JwtRegisteredClaimNames.Sub);

    public string? GetEmail(string token)
        => GetPrincipalFromToken(token)?.FindFirstValue(JwtRegisteredClaimNames.Email);

    public string? GetFullName(string token)
        => GetPrincipalFromToken(token)?.FindFirstValue(JwtRegisteredClaimNames.Name);

    public string? GetRole(string token)
        => GetPrincipalFromToken(token)?.FindFirstValue(ClaimTypes.Role);

    #endregion

    #region From HttpContext

    private ClaimsPrincipal? HttpContextUser => httpContextAccessor.HttpContext?.User;

    public ClaimsPrincipal? CurrentUser => HttpContextUser;

    public string? CurrentUserId => HttpContextUser?.FindFirstValue(JwtRegisteredClaimNames.Sub);

    public string? CurrentUserEmail => HttpContextUser?.FindFirstValue(JwtRegisteredClaimNames.Email);

    public string? CurrentUserFullName => HttpContextUser?.FindFirstValue(JwtRegisteredClaimNames.Name);

    public string? CurrentUserRole => HttpContextUser?.FindFirstValue(ClaimTypes.Role);

    #endregion
}