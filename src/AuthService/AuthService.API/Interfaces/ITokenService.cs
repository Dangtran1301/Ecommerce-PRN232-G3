using AuthService.API.DTOs;
using AuthService.API.Models;

namespace AuthService.API.Interfaces;

public interface ITokenService
{
    (string accessToken, DateTime expiresAt) GenerateAccessToken(User user, UserProfileResponse userProfileDto);

    (string token, DateTime expiresAt) GenerateRefreshToken();
}