using OrderService.API.DTOs;
using OrderService.API.Models;
using SharedKernel.Application.Common;

namespace OrderService.API.Specifications
{
    public class OrderFilterSpecification : BaseSpecification<Order>
    {
        public OrderFilterSpecification(OrderFilterDto filter)
        {
            Criteria = o =>
                (string.IsNullOrEmpty(filter.Keyword) ||
                 o.Id.ToString().Contains(filter.Keyword) ||
                 (o.Status != null && o.Status.ToString().Contains(filter.Keyword))) &&
                (!filter.CustomerId.HasValue || o.CustomerId == filter.CustomerId.Value) &&
                (!filter.MinTotalAmount.HasValue || o.TotalAmount >= filter.MinTotalAmount.Value) &&
                (!filter.MaxTotalAmount.HasValue || o.TotalAmount <= filter.MaxTotalAmount.Value) &&
                (!filter.StartDate.HasValue || o.OrderDate >= filter.StartDate.Value) &&
                (!filter.EndDate.HasValue || o.OrderDate <= filter.EndDate.Value);

            AddInclude(o => o.Items);

            // Sắp xếp
            if (!string.IsNullOrEmpty(filter.OrderBy))
            {
                switch (filter.OrderBy.ToLower())
                {
                    case "date":
                    case "orderdate":
                        if (filter.Descending) ApplyOrderByDescending(o => o.OrderDate);
                        else ApplyOrderBy(o => o.OrderDate);
                        break;

                    case "total":
                    case "totalamount":
                        if (filter.Descending) ApplyOrderByDescending(o => o.TotalAmount);
                        else ApplyOrderBy(o => o.TotalAmount);
                        break;

                    case "status":
                        if (filter.Descending) ApplyOrderByDescending(o => o.Status);
                        else ApplyOrderBy(o => o.Status);
                        break;

                    default:
                        if (filter.Descending) ApplyOrderByDescending(o => o.CreatedAt);
                        else ApplyOrderBy(o => o.CreatedAt);
                        break;
                }
            }

            // Phân trang
            if (filter is { PageIndex: not null, PageSize: not null })
            {
                var skip = (filter.PageIndex.Value - 1) * filter.PageSize.Value;
                var take = filter.PageSize.Value;
                ApplyPaging(skip, take);
            }
        }

        public static implicit operator OrderFilterSpecification(OrderFilterDto filter)
            => new OrderFilterSpecification(filter);
    }
}