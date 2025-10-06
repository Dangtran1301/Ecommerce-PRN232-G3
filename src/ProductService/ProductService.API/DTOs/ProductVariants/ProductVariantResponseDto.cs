namespace CatalogService.DTOs.ProductVariant
{
    
    public class ProductVariantResponseDto
    {
        public Guid Id { get; set; }
        public string VariantName { get; set; } = null!;
        public decimal Price { get; set; }
        public string? Sku { get; set; }
        public string? ImageUrl { get; set; }
    }
}
