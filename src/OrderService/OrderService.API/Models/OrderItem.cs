using SharedKernel.Domain.Common.Entities;

namespace OrderService.API.Models
{
    public class OrderItem : SoftDeleteEntity<Guid>
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        // Navigation property
        public Order Order { get; set; } = null!;
    }
}
