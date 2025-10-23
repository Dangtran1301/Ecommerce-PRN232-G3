using System.ComponentModel.DataAnnotations;

namespace CatalogService.Application.DTOs
{
    public record CreateBrandRequest
    {
        private string _brandName = null!;
        private string? _brandDescription;
        private string? _logoUrl;
        private string? _websiteUrl;

        [Required(ErrorMessage = "Brand name is required")]
        [StringLength(255, ErrorMessage = "Brand name must not exceed 255 characters")]
        public string BrandName
        {
            get => _brandName;
            set => _brandName = value?.Trim() ?? string.Empty;
        }

        [Required(ErrorMessage = "Brand description is required")]
        [StringLength(1000, ErrorMessage = "Brand description must not exceed 1000 characters")]
        public string? BrandDescription
        {
            get => _brandDescription;
            set => _brandDescription = value?.Trim();
        }

        [Required(ErrorMessage = "Brand logo is required")]
        [StringLength(255, ErrorMessage = "Logo URL must not exceed 255 characters")]
        [Url(ErrorMessage = "Logo URL must be a valid URL (e.g., https://example.com/logo.png)")]
        public string? LogoUrl
        {
            get => _logoUrl;
            set => _logoUrl = value?.Trim();
        }

        [Required(ErrorMessage = "Brand website URL is required")]
        [StringLength(255, ErrorMessage = "Website URL must not exceed 255 characters")]
        [Url(ErrorMessage = "Website URL must be a valid URL (e.g., https://example.com)")]
        public string? WebsiteUrl
        {
            get => _websiteUrl;
            set => _websiteUrl = value?.Trim();
        }
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
        private string? _brandName;
        private string? _brandDescription;
        private string? _logoUrl;
        private string? _websiteUrl;

        [Required(ErrorMessage = "Brand name is required")]
        [StringLength(255, ErrorMessage = "Brand name must not exceed 255 characters")]
        public string? BrandName
        {
            get => _brandName;
            set => _brandName = value?.Trim();
        }
        [Required(ErrorMessage = "Brand description is required")]
        [StringLength(1000, ErrorMessage = "Brand description must not exceed 1000 characters")]
        public string? BrandDescription
        {
            get => _brandDescription;
            set => _brandDescription = value?.Trim();
        }
        [Required(ErrorMessage = "Brand logo is required")]
        [StringLength(255, ErrorMessage = "Logo URL must not exceed 255 characters")]
        public string? LogoUrl
        {
            get => _logoUrl;
            set => _logoUrl = value?.Trim();
        }

        [Required(ErrorMessage = "Brand website URL is required")]
        [StringLength(255, ErrorMessage = "Website URL must not exceed 255 characters")]
        [Url(ErrorMessage = "Website URL must be a valid URL (e.g., https://example.com)")]
        public string? WebsiteUrl
        {
            get => _websiteUrl;
            set => _websiteUrl = value?.Trim();
        }
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
