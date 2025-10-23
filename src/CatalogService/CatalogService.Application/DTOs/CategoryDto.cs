using System.ComponentModel.DataAnnotations;

namespace CatalogService.Application.DTOs
{
    public record CreateCategoryRequest
    {
        private string _categoryName = null!;
        private string? _categoryDescription;
        private string? _imageUrl;

        [Required(ErrorMessage = "Category name is required")]
        [StringLength(255, ErrorMessage = "Category name must not exceed 255 characters")]
        public string CategoryName
        {
            get => _categoryName;
            set => _categoryName = value?.Trim() ?? string.Empty;
        }

        [Required(ErrorMessage = "Category description is required")]
        [StringLength(1000, ErrorMessage = "Category description must not exceed 1000 characters")]
        public string? CategoryDescription
        {
            get => _categoryDescription;
            set => _categoryDescription = value?.Trim();
        }

        [Required(ErrorMessage = "Category image is required")]
        [StringLength(255, ErrorMessage = "Image URL must not exceed 255 characters")]
        public string? ImageUrl
        {
            get => _imageUrl;
            set => _imageUrl = value?.Trim();
        }
    }
    public record CategoryDto
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; } = null!;
        public string? CategoryDescription { get; set; }
        public string? ImageUrl { get; set; }
    }
    public record UpdateCategoryRequest
    {
        private string? _categoryName;
        private string? _categoryDescription;
        private string? _imageUrl;
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(255, ErrorMessage = "Category name must not exceed 255 characters")]
        public string? CategoryName
        {
            get => _categoryName;
            set => _categoryName = value?.Trim();
        }
        [Required(ErrorMessage = "Category description is required")]
        [StringLength(1000, ErrorMessage = "Category description must not exceed 1000 characters")]
        public string? CategoryDescription
        {
            get => _categoryDescription;
            set => _categoryDescription = value?.Trim();
        }
        [Required(ErrorMessage = "Category image is required")]
        [StringLength(255, ErrorMessage = "Image URL must not exceed 255 characters")]
        public string? ImageUrl
        {
            get => _imageUrl;
            set => _imageUrl = value?.Trim();
        }
        
    }
    public class CategoryFilterDto
    {
        public string? Keyword { get; set; }
        public int? PageIndex { get; set; } = 1;
        public int? PageSize { get; set; } = 25;
        public string? OrderBy { get; set; } = "CreatedAt";
        public bool Descending { get; set; } = false;
    }
}
