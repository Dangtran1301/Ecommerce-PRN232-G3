using CatalogService.API.DTOs;
using CatalogService.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Application.Common;
using SharedKernel.Application.Extensions;

namespace CatalogService.API.Controllers;

public class ProductAttributesController(IProductAttributeService service) : CatalogControllerBase
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
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductAttributeRequest request)
        => (await service.UpdateAsync(id, request)).ToActionResult();

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
        => (await service.DeleteAsync(id)).ToActionResult();

    //[HttpGet("filter/spec")]
    //public async Task<IActionResult> FilterBySpec([FromQuery] ProductAttributeFilterDto filter)
    //    => (await service.FilterBySpecification(filter)).ToActionResult();

    [HttpPost("filter/dynamic")]
    public async Task<IActionResult> FilterDynamic([FromBody] DynamicQuery query)
        => (await service.FilterByDynamic(query)).ToActionResult();

    [HttpGet("filter/paged")]
    public async Task<IActionResult> FilterPaged([FromQuery] PagedRequest request)
        => (await service.FilterPaged(request)).ToActionResult();
}