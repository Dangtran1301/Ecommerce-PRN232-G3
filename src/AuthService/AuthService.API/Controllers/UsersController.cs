using Asp.Versioning;
using AuthService.API.DTOs;
using AuthService.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Application.Extensions;

namespace AuthService.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion(1.0)]
    [Authorize]
    public class UsersController(IUserService userService) : ControllerBase
    {
        // GET: api/users/{id}
        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin,Seller,Customer")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await userService.GetByIdAsync(id);
            return result.ToActionResult();
        }

        // GET: api/users
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var result = await userService.GetAllAsync();
            return result.ToActionResult();
        }

        // POST: api/users
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] RegisterRequest request, CancellationToken cancellationToken)
        {
            var result = await userService.CreateAsync(request, cancellationToken);
            return result.ToActionResult();
        }

        // PUT: api/users/{id}/role
        [HttpPut("{id:guid}/role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRole(Guid id, [FromBody] RemoteAuthUserRequest request)
        {
            var result = await userService.RemoteRoleAsync(id, request);
            return result.ToActionResult();
        }

        // PUT: api/users/{id}/status
        [HttpPut("{id:guid}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateStatusAuthUserRequest request)
        {
            var result = await userService.UpdateStatusUserAsync(id, request);
            return result.ToActionResult();
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await userService.DeleteAsync(id);
            return result.ToActionResult();
        }

        // POST: api/users/filter
        [HttpPost("filter")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Filter([FromBody] UserFilterRequest filter)
        {
            var result = await userService.FilterBySpecification(filter);
            return result.ToActionResult();
        }
    }
}