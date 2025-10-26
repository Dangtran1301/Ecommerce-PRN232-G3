using AuthService.API.DTOs;

namespace AuthService.API.Interfaces;

public interface ITokenService
{
    (string accessToken, DateTime expiresAt) GenerateAccessToken(UserServiceUserDto user);

    (string token, DateTime expiresAt) GenerateRefreshToken();
}