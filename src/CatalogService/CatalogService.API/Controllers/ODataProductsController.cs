using CatalogService.Application.DTOs.Products;
using CatalogService.Application.Services.Interfaces;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace CatalogService.API.ODataControllers
{
    public class ODataProductsController : ODataController
    {
        private readonly IProductService service;

        public ODataProductsController(IProductService service)
        {
            this.service = service;
        }

        [EnableQuery]
        public IQueryable<ProductDto> Get()
            => service.AsQueryable();
    }
}
