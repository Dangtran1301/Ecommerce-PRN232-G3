namespace CatalogService.Entities
{
    public class Product
    {
        public string ProductId { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string? ProductDescription { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Foreign keys
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public int? BrandId { get; set; }
        public Brand? Brand { get; set; }
    }
}