using CatalogService.Entities;
using SharedKernel.Application.Common;
using System.ComponentModel.DataAnnotations;

namespace CatalogService.Application.DTOs.Stocks
{
    public record CreateStockRequest
    {
        [Required] public Guid ProductId { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [StringLength(255)]
        public string? Location { get; set; }
    }

    public record StockDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public string? Location { get; set; }
    }

    public record UpdateStockRequest
    {
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [StringLength(255)]
        public string? Location { get; set; }
    }

    public class StockFilterDto
    {
        public Guid? ProductId { get; set; }
        public string? Location { get; set; }
        public int? PageIndex { get; set; } = 1;
        public int? PageSize { get; set; } = 25;
        public string? OrderBy { get; set; } = "CreatedAt";
        public bool Descending { get; set; } = false;
    }

    public class StockFilterSpecification : BaseSpecification<Stock>
    {
        public StockFilterSpecification(StockFilterDto filter)
        {
            Criteria = s =>
                (!filter.ProductId.HasValue || s.ProductId == filter.ProductId.Value) &&
                (string.IsNullOrEmpty(filter.Location) || s.Location.Contains(filter.Location));

            switch (filter.OrderBy?.ToLower())
            {
                case "quantity":
                    if (filter.Descending) ApplyOrderByDescending(s => s.Quantity);
                    else ApplyOrderBy(s => s.Quantity);
                    break;

                case "location":
                    if (filter.Descending) ApplyOrderByDescending(s => s.Location);
                    else ApplyOrderBy(s => s.Location);
                    break;

                default:
                    if (filter.Descending) ApplyOrderByDescending(s => s.CreatedAt);
                    else ApplyOrderBy(s => s.CreatedAt);
                    break;
            }

            if (filter.PageIndex is not null && filter.PageSize is not null)
            {
                var skip = (filter.PageIndex.Value - 1) * filter.PageSize.Value;
                var take = filter.PageSize.Value;
                ApplyPaging(skip, take);
            }
        }

        public static implicit operator StockFilterSpecification(StockFilterDto filter)
            => new(filter);
    }
}