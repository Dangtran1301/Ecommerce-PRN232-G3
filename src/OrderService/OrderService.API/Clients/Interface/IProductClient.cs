using System;
using System.Threading.Tasks;
using OrderService.API.DTOs;

namespace OrderService.API.Clients.Interfaces
{
    public interface IProductClient
    {
        Task<ProductDto?> GetProductByIdAsync(Guid productId);
    }
}
