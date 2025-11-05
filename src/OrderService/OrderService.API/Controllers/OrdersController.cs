using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Application.Common;
using SharedKernel.Application.Extensions;
using OrderService.API.DTOs;
using OrderService.API.Services.Interfaces;

namespace OrderService.API.Controllers;

[Route("api/[controller]")]
[Asp.Versioning.ApiVersion("1.0")]
[ApiController]
public class OrdersController(IOrderService service) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
        => (await service.GetByIdAsync(id)).ToActionResult();

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => (await service.GetAllAsync()).ToActionResult();

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
        => (await service.CreateAsync(request)).ToActionResult();

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateOrderRequest request)
        => (await service.UpdateAsync(id, request)).ToActionResult();

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
        => (await service.DeleteAsync(id)).ToActionResult();

    [HttpGet("customer/{customerId:guid}")]
    public async Task<IActionResult> GetByCustomer(Guid customerId)
        => (await service.GetByCustomerIdAsync(customerId)).ToActionResult();

    [HttpGet("{orderId:guid}/items")]
    public async Task<IActionResult> GetOrderWithItems(Guid orderId)
        => (await service.GetOrderWithItemsAsync(orderId)).ToActionResult();

    [HttpGet("paged")]
    public async Task<IActionResult> GetPaged([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        => (await service.GetAllPagedAsync(pageIndex, pageSize)).ToActionResult();

    [HttpGet("paged/metadata")]
    public IActionResult GetPagedMetadata()
    {
        return Ok(new
        {
            entity = "Order",
            defaultPageSize = 10,
            maxPageSize = 100,
            sortableFields = new[] { "OrderDate", "TotalAmount", "CreatedAt" },
            filterableFields = new[]
            {
                new { name = "CustomerId", type = "Guid" },
                new { name = "Status", type = "string" }
            }
        });
    }
}
