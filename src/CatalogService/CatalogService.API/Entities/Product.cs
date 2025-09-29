using SharedKernel.Domain.Common.Entities;

namespace CatalogService.Entities
{
    public class Product : AuditableEntity<Guid>
    {
        public string ProductName { get; set; } = null!;
        public string? ProductDescription { get; set; }

        // Foreign keys
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public int? BrandId { get; set; }
        public Brand? Brand { get; set; }
    }
}