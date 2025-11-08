using CatalogService.Application.DTOs.ProductVariants;
using CatalogService.Application.Services.Interfaces;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace CatalogService.API.ODataControllers;

public class ODataProductVariantsController : ODataController
{
    private readonly IProductVariantService service;

    public ODataProductVariantsController(IProductVariantService service)
    {
        this.service = service;
    }

    [EnableQuery]
    public IQueryable<ProductVariantDto> Get()
        => service.AsQueryable();
}