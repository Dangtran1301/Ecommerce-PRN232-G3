using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Application.Extensions;
using UserService.Application.DTOs;
using UserService.Application.Services.Interfaces;

namespace UserService.API.Controllers;

[Route("api/internal/v{version:apiVersion}/users")]
[ApiVersion("1.0")]
[ApiController]
public class InternalUsersController(IUserService service) : ControllerBase
{
    [HttpPost("validate")]
    public async Task<IActionResult> Validate([FromBody] ValidateUserRequest request, CancellationToken cancellationToken) =>
        (await service.ValidateUser(request.Username, request.Password, cancellationToken)).ToActionResult();

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserById([FromRoute] Guid id) =>
        (await service.GetByIdAsync(id)).ToActionResult();
}