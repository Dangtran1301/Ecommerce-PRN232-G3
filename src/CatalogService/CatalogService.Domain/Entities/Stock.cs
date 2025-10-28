using SharedKernel.Domain.Common.Entities;

namespace CatalogService.Entities
{
    public class Stock : AuditableEntity<Guid>
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }
        public string? Location { get; set; } 
    }
}
