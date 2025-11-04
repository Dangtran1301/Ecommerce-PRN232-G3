using CatalogService.Application.DTOs.Brands;
using CatalogService.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Application.Extensions;

namespace CatalogService.API.Controllers;
public class BrandsController(IBrandService service) : CatalogControllerBase
{
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id)
        => (await service.GetByIdAsync(id)).ToActionResult();

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
        => (await service.GetAllAsync()).ToActionResult();

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateBrandRequest request)
        => (await service.CreateAsync(request)).ToActionResult();

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateBrandRequest request)
        => (await service.UpdateAsync(id, request)).ToActionResult();

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
        => (await service.DeleteAsync(id)).ToActionResult();
}