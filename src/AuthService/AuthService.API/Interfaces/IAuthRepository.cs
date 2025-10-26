using AuthService.API.Models;

namespace AuthService.API.Interfaces;

public interface IAuthRepository
{
    Task<RefreshToken?> GetByRefreshTokenTaskAsync(string refreshToken, CancellationToken cancellationToken = default);
}