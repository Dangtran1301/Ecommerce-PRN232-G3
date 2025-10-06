using CatalogService.DTOs.Brands;
using CatalogService.DTOs.Categories;
using CatalogService.DTOs.ProductAttributes;
using CatalogService.DTOs.Stock;
using ProductService.API.DTOs.ProductVariants;

namespace CatalogService.DTOs.Products
{

    public class ProductResponseDto
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public string? Sku { get; set; }
        public string? Specifications { get; set; }

        public BrandResponseDto? Brand { get; set; }
        public CategoryResponseDto? Category { get; set; }

        public StockResponseDto? Stock { get; set; }
        public List<ProductVariantResponseDto> Variants { get; set; } = new();
        public List<ProductAttributeResponseDto> Attributes { get; set; } = new();
    }
}
