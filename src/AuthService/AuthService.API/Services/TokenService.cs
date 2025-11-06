using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AuthService.API.DTOs;
using AuthService.API.Interfaces;
using AuthService.API.Models;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.API.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly TimeSpan _accessTokenLifetime;
    private readonly TimeSpan _refreshTokenLifetime;

    public TokenService(IConfiguration config)
    {
        _config = config;
        _accessTokenLifetime = TimeSpan.FromMinutes(_config.GetValue<int>("Jwt:AccessTokenLifetimeMinutes"));
        _refreshTokenLifetime = TimeSpan.FromDays(_config.GetValue<int>("Jwt:RefreshTokenLifetimeDays"));
    }

    public (string accessToken, DateTime expiresAt) GenerateAccessToken(User user, UserProfileResponse userProfile)
    {
        var expiresAt = DateTime.UtcNow.Add(_accessTokenLifetime);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, userProfile.FullName),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: expiresAt,
            signingCredentials: creds
        );

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }

    public (string token, DateTime expiresAt) GenerateRefreshToken()
    {
        var randomBytes = RandomNumberGenerator.GetBytes(64);
        var token = Convert.ToBase64String(randomBytes);
        var expiresAt = DateTime.UtcNow.Add(_refreshTokenLifetime);
        return (token, expiresAt);
    }
}