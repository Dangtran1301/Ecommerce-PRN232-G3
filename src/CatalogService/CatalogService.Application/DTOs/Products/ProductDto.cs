using System.ComponentModel.DataAnnotations;

namespace CatalogService.API.DTOs
{
    public record CreateProductRequest
    {
        [Required(ErrorMessage = "Product name is required")]
        [StringLength(255, ErrorMessage = "Product name must not exceed 255 characters")]
        public string ProductName { get; set; } = null!;

        [StringLength(2000, ErrorMessage = "Description must not exceed 2000 characters")]
        public string? Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number")]
        public decimal Price { get; set; }

        [StringLength(100, ErrorMessage = "SKU must not exceed 100 characters")]
        public string? Sku { get; set; }

        [StringLength(255, ErrorMessage = "Image URL must not exceed 255 characters")]
        public string? ImageUrl { get; set; }

        [StringLength(2000, ErrorMessage = "Specifications must not exceed 2000 characters")]
        public string? Specifications { get; set; }

        [Required(ErrorMessage = "Brand ID is required")]
        public Guid BrandId { get; set; }

        [Required(ErrorMessage = "Category ID is required")]
        public Guid? CategoryId { get; set; }
    }

    public record ProductDto
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? Sku { get; set; }
        public string? ImageUrl { get; set; }
        public string? Specifications { get; set; }

        public string? BrandName { get; set; }
        public string? CategoryName { get; set; }

        public int? StockQuantity { get; set; }
    }

    public record UpdateProductRequest
    {
        [StringLength(255, ErrorMessage = "Product name must not exceed 255 characters")]
        public string? ProductName { get; set; }

        [StringLength(2000, ErrorMessage = "Description must not exceed 2000 characters")]
        public string? Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number")]
        public decimal? Price { get; set; }

        [StringLength(100, ErrorMessage = "SKU must not exceed 100 characters")]
        public string? Sku { get; set; }

        [StringLength(255, ErrorMessage = "Image URL must not exceed 255 characters")]
        public string? ImageUrl { get; set; }

        [StringLength(2000, ErrorMessage = "Specifications must not exceed 2000 characters")]
        public string? Specifications { get; set; }

        public Guid? BrandId { get; set; }
        public Guid? CategoryId { get; set; }
    }

    public class ProductFilterDto
    {
        public string? Keyword { get; set; }
        public Guid? BrandId { get; set; }
        public Guid? CategoryId { get; set; }

        public int? PageIndex { get; set; } = 1;
        public int? PageSize { get; set; } = 25;
        public string? OrderBy { get; set; } = "CreatedAt";
        public bool Descending { get; set; } = false;
    }
}