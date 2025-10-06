namespace CatalogService.DTOs.Stock
{


    public class StockResponseDto
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public string? Location { get; set; }
    }
}
