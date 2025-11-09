using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using OrderService.API.Clients.Interfaces;
using OrderService.API.Clients.DTOs;

public class StockClient : IStockClient
{
    private readonly HttpClient _httpClient;

    public StockClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<int> GetStockAsync(Guid productId)
    {
        var response = await _httpClient.GetAsync($"{productId}");
        response.EnsureSuccessStatusCode();

        var stock = await response.Content.ReadFromJsonAsync<StockDto>();
        return stock?.Quantity ?? 0;
    }
}
