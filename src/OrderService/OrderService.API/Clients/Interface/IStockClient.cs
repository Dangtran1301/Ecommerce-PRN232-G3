using System;
using System.Threading.Tasks;

namespace OrderService.API.Clients.Interfaces
{
    public interface IStockClient
    {
        Task<int> GetStockAsync(Guid productId);
    }
}
