using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Application.Common;
using SharedKernel.Application.Extensions;
using UserService.Application.DTOs;
using UserService.Application.Services.Interfaces;

namespace UserService.API.Controllers;

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
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
        => (await service.CreateAsync(request)).ToActionResult();

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateUserRequest request)
        => (await service.UpdateAsync(id, request)).ToActionResult();

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
        => (await service.DeleteAsync(id)).ToActionResult();

    [HttpGet("filter/spec")]
    public async Task<IActionResult> FilterBySpec([FromQuery] UserFilterDto filter)
        => (await service.FilterBySpecification(filter)).ToActionResult();

    [HttpPost("filter/dynamic")]
    public async Task<IActionResult> FilterDynamic([FromBody] DynamicQuery query)
        => (await service.FilterByDynamic(query)).ToActionResult();

    [HttpGet("filter/paged")]
    public async Task<IActionResult> FilterPaged([FromQuery] PagedRequest request)
        => (await service.FilterPaged(request)).ToActionResult();

    [HttpGet("specification/metadata")]
    public IActionResult GetSpecificationFilterMetadata()
    {
        return Ok(new
        {
            entity = "User",
            filterableFields = new[]
            {
                new { name = "FullName", type = "string" },
                new { name = "Email", type = "string" },
                new { name = "Gender", type = "enum" },
                new { name = "Role", type = "enum" },
                new { name = "AccountStatus", type = "enum" }
            },
            sortableFields = new[]
            {
                "FullName",
                "Email",
                "DayOfBirth",
                "CreatedDate"
            }
        });
    }

    [HttpGet("dynamic/metadata")]
    public IActionResult GetDynamicFilterMetadata()
    {
        return Ok(new
        {
            entity = "User",
            filterableFields = new[]
            {
                new { name = "FullName", type = "string" },
                new { name = "Email", type = "string" },
                new { name = "Gender", type = "enum" },
                new { name = "Role", type = "enum" },
                new { name = "DayOfBirth", type = "datetime" },
                new { name = "AccountStatus", type = "enum" }
            },
            operators = Enum.GetNames(typeof(FilterOperator)),
            sortableFields = new[]
            {
                "FullName",
                "Email",
                "DayOfBirth",
                "CreatedDate"
            }
        });
    }

    [HttpGet("paged/metadata")]
    public IActionResult GetPagedMetadata()
    {
        return Ok(new
        {
            defaultPageSize = 10,
            maxPageSize = 100,
            sortableFields = new[]
            {
                "FullName",
                "Email",
                "DayOfBirth",
                "CreatedDate"
            }
        });
    }
}