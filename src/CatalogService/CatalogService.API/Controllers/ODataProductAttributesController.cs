using CatalogService.Application.DTOs.ProductAttributes;
using CatalogService.Application.Services.Interfaces;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace CatalogService.API.ODataControllers;

public class ODataProductAttributesController : ODataController
{
    private readonly IProductAttributeService service;

    public ODataProductAttributesController(IProductAttributeService service)
    {
        this.service = service;
    }

    [EnableQuery]
    public IQueryable<ProductAttributeDto> Get()
        => service.AsQueryable();
}
