using Asp.Versioning;
using CatalogService.Application.Services.Interfaces;
using CatalogService.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Application.Common;
using SharedKernel.Application.Extensions;

namespace CatalogService.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class BrandsController(IBrandService service) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
        => (await service.GetByIdAsync(id)).ToActionResult();

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => (await service.GetAllAsync()).ToActionResult();

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBrandRequest request)
        => (await service.CreateAsync(request)).ToActionResult();

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateBrandRequest request)
        => (await service.UpdateAsync(id, request)).ToActionResult();

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
        => (await service.DeleteAsync(id)).ToActionResult();

    [HttpGet("filter/spec")]
    public async Task<IActionResult> FilterBySpec([FromQuery] BrandFilterDto filter)
        => (await service.FilterBySpecification(filter)).ToActionResult();

    [HttpGet("filter/paged")]
    public async Task<IActionResult> FilterPaged([FromQuery] PagedRequest request)
        => (await service.FilterPaged(request)).ToActionResult();

    [HttpGet("specification/metadata")]
    public IActionResult GetSpecificationFilterMetadata()
    {
        return Ok(new
        {
            entity = "Brand",
            filterableFields = new[]
            {
                new { name = "BrandName", type = "string" },
                new { name = "BrandDescription", type = "string" }
            },
            sortableFields = new[]
            {
                "BrandName",
                "CreatedAt"
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
                "BrandName",
                "CreatedAt"
            }
        });
    }
}