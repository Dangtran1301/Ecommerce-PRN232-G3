using AuthService.API.Interfaces;
using AuthService.API.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Infrastructure.Data.Interfaces;
using SharedKernel.Infrastructure.UnitOfWorks.Repositories;

namespace AuthService.API.Data.UnitOfWorks;

public class AuthRepository(IDbContext dbContext) : EfRepository<RefreshToken, int>(dbContext), IAuthRepository
{
    private readonly DbSet<RefreshToken> _refreshTokens = dbContext.Set<RefreshToken>();

    public async Task<RefreshToken?> GetByRefreshTokenTaskAsync(string refreshToken, CancellationToken cancellationToken = default) =>
       await _refreshTokens.FirstOrDefaultAsync(x => x.Token.Equals(refreshToken), cancellationToken) ?? null;
}