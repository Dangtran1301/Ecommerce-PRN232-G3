namespace CatalogService.DTOs.Categories
{
   

    public class CategoryResponseDto
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string? CategoryDescription { get; set; }
    }
}
