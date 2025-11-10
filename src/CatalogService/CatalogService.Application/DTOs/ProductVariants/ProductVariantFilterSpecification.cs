using CatalogService.Entities;
using SharedKernel.Application.Common;

namespace CatalogService.Application.DTOs.ProductVariants;

public class ProductVariantFilterSpecification : BaseSpecification<ProductVariant>
{
    public ProductVariantFilterSpecification(ProductVariantFilterDto filter)
    {
        Criteria = v =>
            (string.IsNullOrEmpty(filter.Keyword) ||
             v.VariantName.Contains(filter.Keyword) ||
             (v.Sku != null && v.Sku.Contains(filter.Keyword))) &&
            (!filter.ProductId.HasValue || v.ProductId == filter.ProductId.Value);

        switch (filter.OrderBy?.ToLower())
        {
            case "name":
            case "variantname":
                if (filter.Descending) ApplyOrderByDescending(v => v.VariantName);
                else ApplyOrderBy(v => v.VariantName);
                break;

            case "price":
                if (filter.Descending) ApplyOrderByDescending(v => v.Price);
                else ApplyOrderBy(v => v.Price);
                break;

            case "sku":
                if (filter.Descending) ApplyOrderByDescending(v => v.Sku ?? string.Empty);
                else ApplyOrderBy(v => v.Sku ?? string.Empty);
                break;

            default:
                if (filter.Descending) ApplyOrderByDescending(v => v.CreatedAt);
                else ApplyOrderBy(v => v.CreatedAt);
                break;
        }

        if (filter.PageIndex is not null && filter.PageSize is not null)
        {
            var skip = (filter.PageIndex.Value - 1) * filter.PageSize.Value;
            var take = filter.PageSize.Value;
            ApplyPaging(skip, take);
        }
    }

    public static implicit operator ProductVariantFilterSpecification(ProductVariantFilterDto filter)
        => new ProductVariantFilterSpecification(filter);
}