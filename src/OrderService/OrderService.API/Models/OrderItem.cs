using SharedKernel.Domain.Common.Entities;
using SharedKernel.Domain.Common.Entities.Interface;

namespace OrderService.API.Models
{
    public class OrderItem : DefaultEntity<Guid>, IAuditable, ISoftDelete
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; } 
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        // Navigation
        public Order Order { get; set; }

        // Audit & SoftDelete
        public DateTime CreatedAt { get; private set; }
        public string CreatedBy { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public string? UpdatedBy { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTime? DeletedAt { get; private set; }
        public string? DeletedBy { get; private set; }

        public void SetCreated(string user)
        {
            CreatedAt = DateTime.UtcNow;
            CreatedBy = user;
        }

        public void SetUpdated(string user)
        {
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = user;
        }

        public void MarkAsDeleted(string user)
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            DeletedBy = user;
        }

        public void Restore()
        {
            IsDeleted = false;
            DeletedAt = null;
            DeletedBy = null;
        }
    }
}
