namespace CatalogService.DTOs.ProductAttributes
{
   
    public class ProductAttributeResponseDto
    {
        public Guid Id { get; set; }
        public string AttributeName { get; set; } = null!;
        public string AttributeValue { get; set; } = null!;
    }
}
