using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SharedKernel.Application.Extensions;

public class ExtensionAttribute(string[] extensions) : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!extensions.Contains(extension))
            {
                return new ValidationResult($"Only the following extensions are allowed: {string.Join(", ", extensions)}");
            }
        }
        return ValidationResult.Success;
    }
}

[AttributeUsage(AttributeTargets.Property)]
public class ImageDisplayAttribute : Attribute
{
    public string Alt { get; set; } = string.Empty;

    public ImageDisplayAttribute()
    { }

    public ImageDisplayAttribute(string alt)
    {
        Alt = alt;
    }
}

public class NotEqualToAttribute : ValidationAttribute
{
    private readonly string _otherProperty;

    public NotEqualToAttribute(string otherProperty)
    {
        _otherProperty = otherProperty;
        ErrorMessage = "The two fields must not be the same.";
    }

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        var otherProp = validationContext.ObjectType.GetProperty(_otherProperty);
        if (otherProp == null)
            return new ValidationResult($"Unknown property: {_otherProperty}");

        var otherValue = otherProp.GetValue(validationContext.ObjectInstance);

        if (Equals(value, otherValue))
        {
            return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success!;
    }
}