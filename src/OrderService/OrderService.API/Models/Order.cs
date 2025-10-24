namespace OrderService.API.Models

{
    using SharedKernel.Domain.Common.Entities;

    public class Order : SoftDeleteEntity<Guid>
    {
        public Guid CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        // Quan hệ 1-nhiều với OrderItem
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}