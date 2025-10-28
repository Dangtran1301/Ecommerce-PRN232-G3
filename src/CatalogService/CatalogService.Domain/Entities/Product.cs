using CatalogService.Domain.Entities;
using SharedKernel.Domain.Common.Entities;

namespace CatalogService.Entities
{
    public class Product : AuditableEntity<Guid>
    {
        public string ProductName { get; set; } = null!;
        public string? Description { get; set; }

        public decimal Price { get; set; }

        public string? Sku { get; set; }
        public string? ImageUrl { get; set; }
        public string? Specifications { get; set; }

        public Guid BrandId { get; set; }
        public Brand Brand { get; set; } = null!;

        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public Stock Stock { get; set; } = null!;

        public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
        public ICollection<ProductAttribute> Attributes { get; set; } = new List<ProductAttribute>();
    }
}
