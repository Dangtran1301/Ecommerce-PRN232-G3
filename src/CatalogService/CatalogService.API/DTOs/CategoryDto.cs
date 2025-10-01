using System.ComponentModel.DataAnnotations;

namespace CatalogService.API.DTOs
{
    public class CategoryDto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [StringLength(255, ErrorMessage = "Category name must not exceed 255 characters")]
        public string CategoryName { get; set; } = null!;

        [StringLength(1000, ErrorMessage = "Category description must not exceed 1000 characters")]
        public string? CategoryDescription { get; set; }

        [StringLength(255, ErrorMessage = "Image URL must not exceed 255 characters")]
        public string? ImageUrl { get; set; }
    }
}
