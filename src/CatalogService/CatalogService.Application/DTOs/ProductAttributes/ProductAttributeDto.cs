using System.ComponentModel.DataAnnotations;

namespace CatalogService.Application.DTOs.ProductAttributes
{
    public record CreateProductAttributeRequest
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required, StringLength(255)]
        public string AttributeName { get; set; } = null!;

        [Required, StringLength(500)]
        public string AttributeValue { get; set; } = null!;
    }

    public record ProductAttributeDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string AttributeName { get; set; } = null!;
        public string AttributeValue { get; set; } = null!;
    }

    public record UpdateProductAttributeRequest
    {
        [Required, StringLength(255)]
        public string AttributeName { get; set; } = null!;

        [Required, StringLength(500)]
        public string AttributeValue { get; set; } = null!;
    }
}