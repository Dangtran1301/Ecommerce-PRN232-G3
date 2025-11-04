using CatalogService.Domain.Entities;
using SharedKernel.Application.Common;

namespace CatalogService.Application.DTOs.Categories
{
    public class CategoryFilterSpecification : BaseSpecification<Category>
    {
        public CategoryFilterSpecification(CategoryFilterDto filter)
        {
            Criteria = c =>
                string.IsNullOrEmpty(filter.Keyword) ||
                 c.CategoryName.Contains(filter.Keyword) ||
                 c.CategoryDescription != null && c.CategoryDescription.Contains(filter.Keyword);

            if (!string.IsNullOrEmpty(filter.OrderBy))
            {
                switch (filter.OrderBy.ToLower())
                {
                    case "name":
                    case "categoryname":
                        if (filter.Descending) ApplyOrderByDescending(c => c.CategoryName);
                        else ApplyOrderBy(c => c.CategoryName);
                        break;

                    case "description":
                        if (filter.Descending) ApplyOrderByDescending(c => c.CategoryDescription);
                        else ApplyOrderBy(c => c.CategoryDescription);
                        break;

                    default:
                        if (filter.Descending) ApplyOrderByDescending(c => c.CreatedAt);
                        else ApplyOrderBy(c => c.CreatedAt);
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

        public static implicit operator CategoryFilterSpecification(CategoryFilterDto filter)
            => new CategoryFilterSpecification(filter);
    }
}