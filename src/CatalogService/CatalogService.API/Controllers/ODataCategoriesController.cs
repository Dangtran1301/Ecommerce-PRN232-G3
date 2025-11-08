using CatalogService.Application.DTOs.Categories;
using CatalogService.Application.Services.Interfaces;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

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