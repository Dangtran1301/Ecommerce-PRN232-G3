using OrderService.API.Models;
using System.ComponentModel.DataAnnotations;

namespace OrderService.API.DTOs
{
    public record OrderDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;

        public List<OrderItemDto> Items { get; set; } = new();
    }

    public record CreateOrderRequest
    {
        [Required(ErrorMessage = "Customer ID is required")]
        public Guid CustomerId { get; set; }
        public Guid ProductId { get; set; }

        [Required(ErrorMessage = "At least one order item is required")]
        public List<CreateOrderItemRequest> Items { get; set; } = new();
    }

    public record UpdateOrderRequest
    {
        public Guid? CustomerId { get; set; }
        public OrderStatus? Status { get; set; }
    }

    public record OrderItemDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public record CreateOrderItemRequest
    {
        [Required(ErrorMessage = "Product ID is required")]
        public Guid ProductId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be positive")]
        public decimal Price { get; set; }
    }

    public record UpdateOrderItemRequest
    {
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
    }

    public record OrderFilterDto
    {
        public string? Keyword { get; set; }                     
        public Guid? CustomerId { get; set; }                   

        public decimal? MinTotalAmount { get; set; }             
        public decimal? MaxTotalAmount { get; set; }             

        public DateTime? StartDate { get; set; }                 
        public DateTime? EndDate { get; set; }                   

        public string? OrderBy { get; set; }                     
        public bool Descending { get; set; } = false;            

        public int? PageIndex { get; set; } = 1;                 
        public int? PageSize { get; set; } = 10;                 
    }
}
