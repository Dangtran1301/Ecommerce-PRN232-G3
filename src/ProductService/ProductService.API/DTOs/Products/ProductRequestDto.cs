using CatalogService.DTOs.Brands;
using CatalogService.DTOs.Categories;
using CatalogService.DTOs.Category;


namespace CatalogService.DTOs.Product
{
    public class ProductRequestDto
    {
        public string ProductName { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? Sku { get; set; }
        public string? ImageUrl { get; set; }
        public string? Specifications { get; set; }

        public Guid BrandId { get; set; }
        public Guid CategoryId { get; set; }
    }

   
}
