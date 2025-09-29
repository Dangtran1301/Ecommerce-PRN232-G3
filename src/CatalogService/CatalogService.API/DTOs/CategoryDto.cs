namespace CatalogService.API.DTOs
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; } = null!;
        public string CategoryDescription { get; set; }
        public string ImageUrl { get; set; }
    }
}
