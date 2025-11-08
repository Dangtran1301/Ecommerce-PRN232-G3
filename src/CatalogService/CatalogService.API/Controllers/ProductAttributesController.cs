using Asp.Versioning;
using CatalogService.Application.DTOs.ProductAttributes;
using CatalogService.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Application.Common;
using SharedKernel.Application.Extensions;

namespace CatalogService.API.Controllers;

[Route("api/v{version:apiVersion}/catalog/product-attributes")]
[ApiController]
[ApiVersion("1.0")]
public class ProductAttributesController(IProductAttributeService service) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
        => (await service.GetByIdAsync(id)).ToActionResult();

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => (await service.GetAllAsync()).ToActionResult();

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductAttributeRequest request)
        => (await service.CreateAsync(request)).ToActionResult();

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateProductAttributeRequest request)
        => (await service.UpdateAsync(id, request)).ToActionResult();

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
        => (await service.DeleteAsync(id)).ToActionResult();

    [HttpGet("filter/spec")]
    public async Task<IActionResult> FilterBySpec([FromQuery] ProductAttributeFilterDto filter)
        => (await service.FilterBySpecification(filter)).ToActionResult();

    [HttpGet("filter/paged")]
    public async Task<IActionResult> FilterPaged([FromQuery] PagedRequest request)
        => (await service.FilterPaged(request)).ToActionResult();

    [HttpGet("specification/metadata")]
    public IActionResult GetSpecificationFilterMetadata()
    {
        return Ok(new
        {
            entity = "ProductAttribute",
            filterableFields = new[]
            {
                new { name = "ProductId", type = "Guid" },
                new { name = "AttributeName", type = "string" },
                new { name = "AttributeValue", type = "string" }
            },
            sortableFields = new[]
            {
                "AttributeName",
                "AttributeValue",
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
                "AttributeName",
                "AttributeValue",
                "CreatedAt"
            }
        });
    }
}
