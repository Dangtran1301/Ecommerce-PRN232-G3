using CatalogService.Entities;
using SharedKernel.Application.Common;

namespace CatalogService.Application.DTOs.Products
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

            switch (filter.OrderBy?.ToLower())
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

            if (filter.PageIndex is not null && filter.PageSize is not null)
            {
                ApplyPaging((filter.PageIndex.Value - 1) * filter.PageSize.Value, filter.PageSize.Value);
            }
        }

        public static implicit operator ProductFilterSpecification(ProductFilterDto filter)
            => new(filter);
    }
}
