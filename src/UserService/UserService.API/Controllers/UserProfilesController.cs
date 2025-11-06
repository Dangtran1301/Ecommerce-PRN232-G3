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
public class UserProfilesController(IUserProfileService profileService) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
        => (await profileService.GetByIdAsync(id)).ToActionResult();

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => (await profileService.GetAllAsync()).ToActionResult();

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserProfileRequest request)
        => (await profileService.CreateAsync(request)).ToActionResult();

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateUserProfileRequest request)
        => (await profileService.UpdateAsync(id, request)).ToActionResult();

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
        => (await profileService.DeleteAsync(id)).ToActionResult();

    [HttpGet("filter/spec")]
    public async Task<IActionResult> FilterBySpec([FromQuery] UserProfileFilterDto filter)
        => (await profileService.FilterBySpecification(filter)).ToActionResult();

    [HttpPost("filter/dynamic")]
    public async Task<IActionResult> FilterDynamic([FromBody] DynamicQuery query)
        => (await profileService.FilterByDynamic(query)).ToActionResult();

    [HttpGet("filter/paged")]
    public async Task<IActionResult> FilterPaged([FromQuery] PagedRequest request)
        => (await profileService.FilterPaged(request)).ToActionResult();

    [HttpGet("specification/metadata")]
    public IActionResult GetSpecificationFilterMetadata() =>
        Ok(new
        {
            entity = "UserProfileAutomapperProfile",
            filterableFields = new[]
            {
                new { name = "FullName", type = "string" },
                new { name = "PhoneNumber", type = "string" },
                new { name = "Gender", type = "enum" },
                new { name = "AccountStatus", type = "enum" },
                new { name = "DayOfBirth", type = "datetime" }
            },
            sortableFields = new[]
            {
                "FullName",
                "DayOfBirth",
                "CreatedAt"
            }
        });

    [HttpGet("dynamic/metadata")]
    public IActionResult GetDynamicFilterMetadata() =>
        Ok(new
        {
            entity = "UserProfileAutomapperProfile",
            filterableFields = new[]
            {
                new { name = "FullName", type = "string" },
                new { name = "PhoneNumber", type = "string" },
                new { name = "Gender", type = "enum" },
                new { name = "AccountStatus", type = "enum" },
                new { name = "DayOfBirth", type = "datetime" }
            },
            operators = Enum.GetNames(typeof(FilterOperator)),
            sortableFields = new[]
            {
                "FullName",
                "DayOfBirth",
                "CreatedAt"
            }
        });

    [HttpGet("paged/metadata")]
    public IActionResult GetPagedMetadata() =>
        Ok(new
        {
            defaultPageSize = 10,
            maxPageSize = 100,
            sortableFields = new[]
            {
                "FullName",
                "DayOfBirth",
                "CreatedAt"
            }
        });
}