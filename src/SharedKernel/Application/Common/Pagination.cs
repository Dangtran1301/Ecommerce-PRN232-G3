namespace SharedKernel.Application.Common;

public class PagedRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public int Skip => (PageNumber - 1) * PageSize;
    public int Take => PageSize;
}

public class PagedResult<T>(IReadOnlyList<T> items, int totalCount, int pageNumber, int pageSize)
{
    public IReadOnlyList<T> Items { get; } = items;
    public int TotalCount { get; } = totalCount;
    public int PageNumber { get; } = pageNumber;
    public int PageSize { get; } = pageSize;

    public bool HasNextPage => PageNumber * PageSize < TotalCount;
    public bool HasPreviousPage => PageNumber > 1;
}