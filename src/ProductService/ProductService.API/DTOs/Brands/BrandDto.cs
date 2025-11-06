using System.ComponentModel.DataAnnotations;

namespace CatalogService.API.DTOs
{
    public record CreateBrandRequest
    {
        [Required(ErrorMessage = "Brand name is required")]
        [StringLength(255, ErrorMessage = "Brand name must not exceed 255 characters")]
        public string BrandName { get; set; } = null!;
        [Required(ErrorMessage = "Brand description is required")]
        [StringLength(1000, ErrorMessage = "Brand description must not exceed 1000 characters")]
        public string? BrandDescription { get; set; }
        [Required(ErrorMessage = "Brand logo is required")]
        [StringLength(255, ErrorMessage = "Logo URL must not exceed 255 characters")]
        public string? LogoUrl { get; set; }
        [Required(ErrorMessage = "Brand website URL is required")]
        [StringLength(255, ErrorMessage = "Website URL must not exceed 255 characters")]
        public string? WebsiteUrl { get; set; }
    }
    public record BrandDto
    {
        public Guid Id { get; set; }
        public string BrandName { get; set; } = null!;
        public string? BrandDescription { get; set; }
        public string? LogoUrl { get; set; }
        public string? WebsiteUrl { get; set; }
    }
    public record UpdateBrandRequest
    {
        [StringLength(255, ErrorMessage = "Brand name must not exceed 255 characters")]
        public string? BrandName { get; set; }

        [StringLength(1000, ErrorMessage = "Brand description must not exceed 1000 characters")]
        public string? BrandDescription { get; set; }

        [StringLength(255, ErrorMessage = "Logo URL must not exceed 255 characters")]
        public string? LogoUrl { get; set; }

        [StringLength(255, ErrorMessage = "Website URL must not exceed 255 characters")]
        public string? WebsiteUrl { get; set; }
    }
    public class BrandFilterDto
    {
        public string? Keyword { get; set; }
        public int? PageIndex { get; set; } = 1;
        public int? PageSize { get; set; } = 25;
        public string? OrderBy { get; set; } = "CreatedAt";
        public bool Descending { get; set; } = false;
    }
}
