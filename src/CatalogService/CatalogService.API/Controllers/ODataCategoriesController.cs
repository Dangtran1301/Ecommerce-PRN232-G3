using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;
using CatalogService.Application.DTOs;
using CatalogService.Application.Services.Interfaces;

namespace CatalogService.API.ODataControllers;

public class ODataCategoriesController : ODataController
{
    private readonly ICategoryService service;

    public ODataCategoriesController(ICategoryService service)
    {
        this.service = service;
    }

    [EnableQuery]
    public IQueryable<CategoryDto> Get()
        => service.AsQueryable();
}
