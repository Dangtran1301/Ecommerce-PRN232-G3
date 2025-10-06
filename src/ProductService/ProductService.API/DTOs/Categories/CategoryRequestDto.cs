namespace CatalogService.DTOs.Category
{
    public class CategoryRequestDto
    {
        public string CategoryName { get; set; } = null!;
        public string? CategoryDescription { get; set; }
        public string? ImageUrl { get; set; }
    }

  
}
