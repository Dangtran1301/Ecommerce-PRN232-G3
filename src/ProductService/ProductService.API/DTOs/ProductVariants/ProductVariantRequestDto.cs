namespace ProductService.API.DTOs.ProductVariants
{
    public class ProductVariantRequestDto
    {
        public string VariantName { get; set; } = null!;
        public decimal Price { get; set; }
        public string? Sku { get; set; }
        public string? ImageUrl { get; set; }
    }

   
    
}
