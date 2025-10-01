using System.ComponentModel.DataAnnotations;

namespace CatalogService.API.DTOs
{
    public record BrandDto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Brand name is required")]
        [StringLength(255, ErrorMessage = "Brand name must not exceed 255 characters")]
        public string BrandName { get; set; } = null!;

        [StringLength(1000, ErrorMessage = "Brand description must not exceed 1000 characters")]
        public string? BrandDescription { get; set; }

        [StringLength(255, ErrorMessage = "Logo URL must not exceed 255 characters")]
        public string? LogoUrl { get; set; }

        [StringLength(255, ErrorMessage = "Website URL must not exceed 255 characters")]
        public string? WebsiteUrl { get; set; }
    }
}
