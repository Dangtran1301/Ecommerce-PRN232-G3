using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;
using CatalogService.Application.DTOs;
using CatalogService.Application.Services.Interfaces;

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
