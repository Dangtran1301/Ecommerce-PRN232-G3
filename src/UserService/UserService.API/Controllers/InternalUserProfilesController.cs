using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Application.Extensions;
using UserService.Application.DTOs;
using UserService.Application.Services.Interfaces;

namespace UserService.API.Controllers;

[Route("api/internal/v{version:apiVersion}/users")]
[ApiVersion("1.0")]
[ApiController]
public class InternalUserProfilesController(IUserProfileService profileService) : ControllerBase
{
    [HttpGet("{id:guid}")]
    [ActionName(nameof(GetUserProfileById))]
    public async Task<IActionResult> GetUserProfileById([FromRoute] Guid id) =>
        (await profileService.GetByIdAsync(id)).ToActionResult();

    [HttpPost]
    [ActionName(nameof(CreateFromAuth))]
    public async Task<IActionResult> CreateFromAuth([FromBody] CreateUserProfileFromAuthRequest request, CancellationToken cancellationToken)
    {
        var result = await profileService.CreateFromAuthAsync(
            request.UserId,
            request.FullName,
            request.PhoneNumber,
            cancellationToken: cancellationToken);
        return result.ToActionResult();
    }

    [HttpDelete("{id:guid}")]
    [ActionName(nameof(DeleteFromAuth))]
    public async Task<IActionResult> DeleteFromAuth(Guid id, CancellationToken cancellationToken) =>
        (await profileService.DeleteAsync(id, cancellationToken)).ToActionResult();
}