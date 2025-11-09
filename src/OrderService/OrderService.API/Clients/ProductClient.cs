using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using OrderService.API.Clients.Interfaces;
using OrderService.API.DTOs;

namespace OrderService.API.Clients
{
    public class ProductClient : IProductClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProductClient> _logger;
        private readonly string _catalogBaseUrl;

        public ProductClient(HttpClient httpClient, IConfiguration configuration, ILogger<ProductClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _catalogBaseUrl = configuration["Services:CatalogServiceUrl"]
                ?? throw new ArgumentNullException("Services:CatalogServiceUrl", "CatalogServiceUrl is missing in configuration.");
        }

        public async Task<ProductDto?> GetProductByIdAsync(Guid productId)
        {
            try
            {
                var url = $"{_catalogBaseUrl}/api/v1/catalog/Products/{productId}";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Failed to fetch product {ProductId} from CatalogService. StatusCode: {StatusCode}",
                        productId, response.StatusCode);
                    return null;
                }

                var product = await response.Content.ReadFromJsonAsync<ProductDto>();
                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling CatalogService for product {ProductId}", productId);
                return null;
            }
        }

        public async Task<int> GetStockAsync(Guid productId)
        {
            var response = await _httpClient.GetAsync($"{productId}/stock");
            response.EnsureSuccessStatusCode();

            var stockStr = await response.Content.ReadAsStringAsync();
            return int.Parse(stockStr);
        }
    }
}
