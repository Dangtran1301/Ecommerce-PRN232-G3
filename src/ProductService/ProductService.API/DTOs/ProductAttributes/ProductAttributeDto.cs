using System.ComponentModel.DataAnnotations;

namespace CatalogService.API.DTOs
{
    public record CreateProductAttributeRequest
    {
        [Required][StringLength(100)] public string AttributeName { get; set; } = null!;
        [Required][StringLength(255)] public string AttributeValue { get; set; } = null!;
        [Required] public Guid ProductId { get; set; }
    }

    public record ProductAttributeDto
    {
        public Guid Id { get; set; }
        public string AttributeName { get; set; } = null!;
        public string AttributeValue { get; set; } = null!;
    }

    public record UpdateProductAttributeRequest
    {
        [StringLength(100)] public string? AttributeName { get; set; }
        [StringLength(255)] public string? AttributeValue { get; set; }
    }
}
