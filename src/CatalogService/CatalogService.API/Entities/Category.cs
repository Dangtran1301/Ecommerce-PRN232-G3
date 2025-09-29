using SharedKernel.Domain.Common.Entities;

namespace CatalogService.Entities
{
    public class Category : AuditableEntity<Guid>
    {
        public string CategoryName { get; set; } = null!;
        public string? CategoryDescription { get; set; }
        public string? ImageUrl { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}