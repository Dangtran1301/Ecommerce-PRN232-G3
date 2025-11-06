using CatalogService.Application.DTOs.Brands;
using CatalogService.Application.Services.Interfaces;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace CatalogService.API.ODataControllers;

public class ODataBrandsController : ODataController
{
    private readonly IBrandService service;

    public ODataBrandsController(IBrandService service)
    {
        this.service = service;
    }

    [EnableQuery]
    public IQueryable<BrandDto> Get()
        => service.AsQueryable();
}