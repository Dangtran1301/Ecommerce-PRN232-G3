using Asp.Versioning;
using AuthService.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;

namespace AuthService.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class MaintenanceController : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpPost("cleanup")]
        public async Task<IActionResult> Cleanup([FromServices] IRepository<RefreshToken, int> repo, CancellationToken cancellationToken)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-7);
            var oldTokens = await repo.AsQueryable()
                .Where(x => (x.IsRevoked || x.ExpiryDate < DateTime.UtcNow) && x.ExpiryDate < cutoffDate)
                .ToListAsync(cancellationToken);

            foreach (var token in oldTokens)
                await repo.Remove(token, cancellationToken);

            return Ok(new { deleted = oldTokens.Count });
        }
    }
}