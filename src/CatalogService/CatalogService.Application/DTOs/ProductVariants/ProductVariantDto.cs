using System.ComponentModel.DataAnnotations;

namespace CatalogService.API.DTOs
{
    public record CreateProductVariantRequest
    {
        [Required(ErrorMessage = "Variant name is required")]
        [StringLength(255, ErrorMessage = "Variant name must not exceed 255 characters")]
        public string VariantName { get; set; } = null!;

        [Range(0, double.MaxValue, ErrorMessage = "Price must be positive")]
        public decimal Price { get; set; }

        [StringLength(100, ErrorMessage = "SKU must not exceed 100 characters")]
        public string? Sku { get; set; }

        [StringLength(255, ErrorMessage = "Image URL must not exceed 255 characters")]
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Product ID is required")]
        public Guid ProductId { get; set; }
    }

    public record ProductVariantDto
    {
        public Guid Id { get; set; }
        public string VariantName { get; set; } = null!;
        public decimal Price { get; set; }
        public string? Sku { get; set; }
        public string? ImageUrl { get; set; }
    }

    public record UpdateProductVariantRequest
    {
        [StringLength(255)] public string? VariantName { get; set; }
        [Range(0, double.MaxValue)] public decimal? Price { get; set; }
        [StringLength(100)] public string? Sku { get; set; }
        [StringLength(255)] public string? ImageUrl { get; set; }
    }

    public class ProductVariantFilterDto
    {
        public string? Keyword { get; set; }
        public Guid? ProductId { get; set; }
        public int? PageIndex { get; set; } = 1;
        public int? PageSize { get; set; } = 25;
        public string? OrderBy { get; set; } = "CreatedAt";
        public bool Descending { get; set; } = false;
    }
}