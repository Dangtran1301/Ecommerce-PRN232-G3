using SharedKernel.Domain.Common.Entities;

namespace CatalogService.Entities
{
    public class ProductAttribute : AuditableEntity<Guid>
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public string AttributeName { get; set; } = null!;
        public string AttributeValue { get; set; } = null!;
    }
}