using CatalogService.API.DTOs;
using CatalogService.Entities;
using SharedKernel.Application.Common;

namespace CatalogService.API.Specifications
{
    public class ProductVariantFilterSpecification : BaseSpecification<ProductVariant>
    {
        public ProductVariantFilterSpecification(ProductVariantFilterDto filter)
        {
            Criteria = v =>
                (string.IsNullOrEmpty(filter.Keyword) || v.VariantName.Contains(filter.Keyword)) &&
                (!filter.ProductId.HasValue || v.ProductId == filter.ProductId.Value);

            AddInclude(v => v.Product);

            if (!string.IsNullOrEmpty(filter.OrderBy))
            {
                switch (filter.OrderBy.ToLower())
                {
                    case "variantname":
                        if (filter.Descending) ApplyOrderByDescending(v => v.VariantName);
                        else ApplyOrderBy(v => v.VariantName);
                        break;

                    case "price":
                        if (filter.Descending) ApplyOrderByDescending(v => v.Price);
                        else ApplyOrderBy(v => v.Price);
                        break;

                    default:
                        if (filter.Descending) ApplyOrderByDescending(v => v.CreatedAt);
                        else ApplyOrderBy(v => v.CreatedAt);
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

        public static implicit operator ProductVariantFilterSpecification(ProductVariantFilterDto filter)
            => new ProductVariantFilterSpecification(filter);
    }
}
