using CatalogService.Entities;
using SharedKernel.Domain.Common.Entities;

namespace CatalogService.Domain.Entities
{
    public class Brand : AuditableEntity<Guid>
    {
        public string BrandName { get; set; } = null!;
        public string? BrandDescription { get; set; }
        public string? WebsiteUrl { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}