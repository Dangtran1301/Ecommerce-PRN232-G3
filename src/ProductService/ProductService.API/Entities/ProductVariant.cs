using SharedKernel.Domain.Common.Entities;

namespace CatalogService.Entities
{
    public class ProductVariant : AuditableEntity<Guid>
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public string VariantName { get; set; } = null!;  
        public decimal Price { get; set; }               
        public string? Sku { get; set; }
        public string? ImageUrl { get; set; }
    }
}
