using System.ComponentModel.DataAnnotations;

namespace CatalogService.API.DTOs
{
    public record CreateStockRequest
    {
        [Required] public Guid ProductId { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be non-negative")]
        public int Quantity { get; set; }

        [StringLength(255)] public string? Location { get; set; }
    }

    public record StockDto
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public string? Location { get; set; }
    }

    public record UpdateStockRequest
    {
        [Range(0, int.MaxValue)] public int? Quantity { get; set; }
        [StringLength(255)] public string? Location { get; set; }
    }
}
