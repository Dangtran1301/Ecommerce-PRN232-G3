namespace OrderService.API.Clients.DTOs
{
    public record StockDto
    {
        public Guid Id { get; init; }
        public int Quantity { get; init; }
        public string? Location { get; init; }
    }
}
