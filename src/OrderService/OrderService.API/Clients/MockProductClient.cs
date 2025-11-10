using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OrderService.API.Clients.Interfaces;
using OrderService.API.DTOs;

namespace OrderService.API.Clients.Mocks
{
    public class MockProductClient : IProductClient
    {
        private readonly Dictionary<Guid, (ProductDto Product, int Stock)> _mockData;

        public MockProductClient()
        {
            _mockData = new Dictionary<Guid, (ProductDto, int)>
            {
                {
                    Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    (
                        new ProductDto
                        {
                            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                            ProductName = "Mock Product A",
                            Description = "Sample product A for testing",
                            Price = 100,
                            Sku = "A100",
                            ImageUrl = "https://via.placeholder.com/150"
                        },
                        5 // còn 5 sản phẩm trong kho
                    )
                },
                {
                    Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    (
                        new ProductDto
                        {
                            Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                            ProductName = "Mock Product B",
                            Description = "Sample product B for testing",
                            Price = 50,
                            Sku = "B200",
                            ImageUrl = "https://via.placeholder.com/150"
                        },
                        0 // hết hàng
                    )
                }
            };
        }

        public Task<ProductDto?> GetProductByIdAsync(Guid productId)
        {
            if (_mockData.TryGetValue(productId, out var product))
                return Task.FromResult<ProductDto?>(product.Product);

            return Task.FromResult<ProductDto?>(null);
        }

        public Task<int> GetStockAsync(Guid productId)
        {
            if (_mockData.TryGetValue(productId, out var product))
                return Task.FromResult(product.Stock);

            return Task.FromResult(0);
        }
    }
}
