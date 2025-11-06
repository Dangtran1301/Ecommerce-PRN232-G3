using CatalogService.Entities;
using SharedKernel.Application.Common;

namespace CatalogService.Application.DTOs.ProductAttributes;

public class ProductAttributeFilterSpecification : BaseSpecification<ProductAttribute>
{
    public ProductAttributeFilterSpecification(ProductAttributeFilterDto filter)
    {
        Criteria = a =>
            (string.IsNullOrEmpty(filter.Keyword) ||
             a.AttributeName.Contains(filter.Keyword) ||
             a.AttributeValue.Contains(filter.Keyword)) &&
            (!filter.ProductId.HasValue || a.ProductId == filter.ProductId.Value);

        if (!string.IsNullOrEmpty(filter.OrderBy))
        {
            switch (filter.OrderBy.ToLower())
            {
                case "name":
                case "attributename":
                    if (filter.Descending) ApplyOrderByDescending(a => a.AttributeName);
                    else ApplyOrderBy(a => a.AttributeName);
                    break;

                case "value":
                case "attributevalue":
                    if (filter.Descending) ApplyOrderByDescending(a => a.AttributeValue);
                    else ApplyOrderBy(a => a.AttributeValue);
                    break;

                default:
                    if (filter.Descending) ApplyOrderByDescending(a => a.CreatedAt);
                    else ApplyOrderBy(a => a.CreatedAt);
                    break;
            }
        }

        if (filter is { PageIndex: not null, PageSize: not null })
        {
            var skip = (filter.PageIndex.Value - 1) * filter.PageSize.Value;
            var take = filter.PageSize.Value;
            ApplyPaging(skip, take);
        }
    }

    public static implicit operator ProductAttributeFilterSpecification(ProductAttributeFilterDto filter)
        => new ProductAttributeFilterSpecification(filter);
}

