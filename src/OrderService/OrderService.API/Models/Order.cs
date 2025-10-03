namespace OrderService.API.Models

 
{
    using SharedKernel.Domain.Common.Entities;
    using SharedKernel.Domain.Common.Entities.Interface;

    public class Order : DefaultEntity<Guid>, IAuditable, ISoftDelete
    {
        public Guid CustomerId { get; set; }   
        public DateTime OrderDate { get; set; } 
        public decimal TotalAmount { get; set; } 

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        // Quan hệ 1-nhiều với OrderItem
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

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
