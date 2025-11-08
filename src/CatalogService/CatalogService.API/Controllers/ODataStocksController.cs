using CatalogService.Application.DTOs.Stocks;
using CatalogService.Application.Services.Interfaces;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace CatalogService.API.ODataControllers;

public class ODataStocksController : ODataController
{
    private readonly IStockService service;

    public ODataStocksController(IStockService service)
    {
        this.service = service;
    }

    [EnableQuery]
    public IQueryable<StockDto> Get() => service.AsQueryable();
}