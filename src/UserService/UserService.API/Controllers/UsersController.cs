using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Application.Extensions;
using UserService.Application.DTOs;
using UserService.Application.Services.Interfaces;

namespace UserService.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class UsersController(IUserService service) : ControllerBase
    {
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
            => (await service.GetByIdAsync(id)).ToActionResult();

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => (await service.GetAllAsync()).ToActionResult();

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserRequest request)
            => (await service.CreateAsync(request)).ToActionResult();

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateUserRequest request)
            => (await service.UpdateAsync(id, request)).ToActionResult();

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
            => (await service.DeleteAsync(id)).ToActionResult();
    }
}