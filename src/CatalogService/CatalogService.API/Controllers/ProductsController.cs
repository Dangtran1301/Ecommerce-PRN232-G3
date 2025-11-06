using CatalogService.API.Controllers;
using CatalogService.API.DTOs;
using CatalogService.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Application.Common;
using SharedKernel.Application.Extensions;

namespace ProductService.API.Controllers;

public class ProductsController(IProductService service) : CatalogControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
        => (await service.GetByIdAsync(id)).ToActionResult();

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => (await service.GetAllAsync()).ToActionResult();

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
        => (await service.CreateAsync(request)).ToActionResult();

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductRequest request)
        => (await service.UpdateAsync(id, request)).ToActionResult();

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
        => (await service.DeleteAsync(id)).ToActionResult();

    [HttpGet("filter/spec")]
    public async Task<IActionResult> FilterBySpec([FromQuery] ProductFilterDto filter)
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
            entity = "Product",
            filterableFields = new[]
            {
                new { name = "ProductName", type = "string" },
                new { name = "Price", type = "decimal" },
                new { name = "Sku", type = "string" }
            },
            sortableFields = new[]
            {
                "ProductName", "Price", "CreatedAt"
            }
        });
    }

    [HttpGet("dynamic/metadata")]
    public IActionResult GetDynamicFilterMetadata()
    {
        return Ok(new
        {
            entity = "Product",
            filterableFields = new[]
            {
                new { name = "ProductName", type = "string" },
                new { name = "Description", type = "string" },
                new { name = "Price", type = "decimal" },
                new { name = "Sku", type = "string" }
            },
            operators = Enum.GetNames(typeof(FilterOperator)),
            sortableFields = new[]
            {
                "ProductName", "Price", "CreatedAt"
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
                "ProductName", "Price", "CreatedAt"
            }
        });
    }
}