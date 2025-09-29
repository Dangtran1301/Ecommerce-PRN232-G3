namespace CatalogService.API.DTOs
{
    public record BrandDto
    {
        public Guid Id { get; set; }
        public string BrandName { get; set; } = null!;
        public string BrandDescription { get; set; }
        public string LogoUrl { get; set; }
        public string WebsiteUrl { get; set; }
    }
}
