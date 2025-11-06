using AuthService.API.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;

namespace AuthService.API.Services;

public class TokenCleanupService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<TokenCleanupService> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromHours(6);
    private readonly int _retentionDays = 7;

    public TokenCleanupService(IServiceScopeFactory scopeFactory, ILogger<TokenCleanupService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("✅ Token cleanup service started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var repository = scope.ServiceProvider.GetRequiredService<IRepository<RefreshToken, int>>();

                var cutoffDate = DateTime.UtcNow.AddDays(-_retentionDays);
                var expiredTokens = await repository.AsQueryable()
                    .Where(x => (x.IsRevoked || x.ExpiryDate < DateTime.UtcNow) && x.ExpiryDate < cutoffDate)
                    .ToListAsync(stoppingToken);

                if (expiredTokens.Count > 0)
                {
                    foreach (var token in expiredTokens)
                        await repository.Remove(token, stoppingToken);

                    _logger.LogInformation("🧹 Deleted {Count} expired/revoked tokens older than {Days} days.", expiredTokens.Count, _retentionDays);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during token cleanup.");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }
}