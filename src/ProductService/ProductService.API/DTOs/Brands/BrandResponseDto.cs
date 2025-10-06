namespace CatalogService.DTOs.Brands
{
    
    public class BrandResponseDto
    {
        public Guid Id { get; set; }
        public string BrandName { get; set; } = null!;
        public string? LogoUrl { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? BrandDescription { get; set; }
    }
}