using CatalogService.Domain.Entities;
using SharedKernel.Application.Common;

namespace CatalogService.Application.DTOs.Brands
{
    public class BrandFilterSpecification : BaseSpecification<Brand>
    {
        public BrandFilterSpecification(BrandFilterDto filter)
        {
            Criteria = b =>
                string.IsNullOrEmpty(filter.Keyword) ||
                 b.BrandName.Contains(filter.Keyword) ||
                 b.BrandDescription != null && b.BrandDescription.Contains(filter.Keyword);

            if (!string.IsNullOrEmpty(filter.OrderBy))
            {
                switch (filter.OrderBy.ToLower())
                {
                    case "name":
                    case "brandname":
                        if (filter.Descending) ApplyOrderByDescending(b => b.BrandName);
                        else ApplyOrderBy(b => b.BrandName);
                        break;

                    case "description":
                        if (filter.Descending) ApplyOrderByDescending(b => b.BrandDescription);
                        else ApplyOrderBy(b => b.BrandDescription);
                        break;

                    default:
                        if (filter.Descending) ApplyOrderByDescending(b => b.CreatedAt);
                        else ApplyOrderBy(b => b.CreatedAt);
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

        public static implicit operator BrandFilterSpecification(BrandFilterDto filter)
            => new BrandFilterSpecification(filter);
    }
}