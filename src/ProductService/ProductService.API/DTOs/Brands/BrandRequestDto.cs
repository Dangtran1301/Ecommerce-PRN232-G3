namespace CatalogService.DTOs.Brands
{
    public class BrandRequestDto
    {
        public string BrandName { get; set; } = null!;
        public string? BrandDescription { get; set; }
        public string? LogoUrl { get; set; }
        public string? WebsiteUrl { get; set; }
    }
}