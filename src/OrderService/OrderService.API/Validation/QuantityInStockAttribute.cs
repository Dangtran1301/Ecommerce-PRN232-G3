using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using OrderService.API.Clients.Interfaces;

public class QuantityInStockAttribute : ValidationAttribute
{
    private readonly string _productIdPropertyName;

    public QuantityInStockAttribute(string productIdPropertyName)
    {
        _productIdPropertyName = productIdPropertyName;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
            return new ValidationResult("Quantity không được để trống");

        // Lấy property ProductId của cùng DTO
        var productProperty = validationContext.ObjectType.GetProperty(_productIdPropertyName);
        if (productProperty == null)
            throw new Exception($"Cannot find property {_productIdPropertyName} on {validationContext.ObjectType.Name}");

        var productId = (Guid)productProperty.GetValue(validationContext.ObjectInstance)!;

        // Lấy IProductClient từ DI
        var productClient = validationContext.GetService<IProductClient>();
        if (productClient == null)
            throw new Exception("Cannot get IProductClient from DI");

        // Lấy stock từ CatalogService
        var stock = productClient.GetStockAsync(productId).Result;

        var quantity = (int)value;
        if (quantity > stock)
            return new ValidationResult($"Sản phẩm {productId} chỉ còn {stock} trong kho, không thể đặt {quantity}");

        return ValidationResult.Success;
    }
}
