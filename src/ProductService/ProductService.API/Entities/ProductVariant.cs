using SharedKernel.Domain.Common.Entities;

namespace CatalogService.Entities
{
    public class ProductVariant : AuditableEntity<Guid>
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public string VariantName { get; set; } = null!;  // Ví dụ: "16GB RAM + 512GB SSD"
        public decimal Price { get; set; }               // Giá riêng của variant
        public string? Sku { get; set; }
        public string? ImageUrl { get; set; }
    }
}
