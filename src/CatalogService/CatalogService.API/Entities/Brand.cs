using SharedKernel.Domain.Common.Entities;

namespace CatalogService.Entities
{
    public class Brand : AuditableEntity<Guid>
    {
        public string BrandName { get; set; } = null!;
        public string? BrandDescription { get; set; }
        public string? LogoUrl { get; set; }
        public string? WebsiteUrl { get; set; }
    }
}