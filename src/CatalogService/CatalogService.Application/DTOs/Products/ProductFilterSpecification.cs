using CatalogService.API.DTOs;
using CatalogService.Entities;
using SharedKernel.Application.Common;

namespace CatalogService.API.Specifications
{
    public class ProductFilterSpecification : BaseSpecification<Product>
    {
        public ProductFilterSpecification(ProductFilterDto filter)
        {
            Criteria = p =>
                (string.IsNullOrEmpty(filter.Keyword) ||
                 p.ProductName.Contains(filter.Keyword) ||
                 (p.Description != null && p.Description.Contains(filter.Keyword))) &&
                (!filter.BrandId.HasValue || p.BrandId == filter.BrandId.Value) &&
                (!filter.CategoryId.HasValue || p.CategoryId == filter.CategoryId.Value);

            AddInclude(p => p.Brand);
            AddInclude(p => p.Category);
            AddInclude(p => p.Stock);

            if (!string.IsNullOrEmpty(filter.OrderBy))
            {
                switch (filter.OrderBy.ToLower())
                {
                    case "name":
                    case "productname":
                        if (filter.Descending) ApplyOrderByDescending(p => p.ProductName);
                        else ApplyOrderBy(p => p.ProductName);
                        break;

                    case "price":
                        if (filter.Descending) ApplyOrderByDescending(p => p.Price);
                        else ApplyOrderBy(p => p.Price);
                        break;

                    default:
                        if (filter.Descending) ApplyOrderByDescending(p => p.CreatedAt);
                        else ApplyOrderBy(p => p.CreatedAt);
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

        public static implicit operator ProductFilterSpecification(ProductFilterDto filter)
            => new ProductFilterSpecification(filter);
    }
}