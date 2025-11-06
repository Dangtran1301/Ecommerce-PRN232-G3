using Microsoft.EntityFrameworkCore;
using SharedKernel.Application.Common;

namespace SharedKernel.Application.Extensions;

public static class PagedResultExtensions
{
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        PagedRequest request,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip(request.Skip)
            .Take(request.Take)
            .ToListAsync(cancellationToken);

        return new PagedResult<T>(items, totalCount, request.PageNumber, request.PageSize);
    }

    public static PagedResult<T> ToPagedResult<T>(
        this IEnumerable<T> source,
        PagedRequest request)
    {
        var enumerable = source as T[] ?? source.ToArray();
        var totalCount = enumerable.Count();

        var items = enumerable
            .Skip(request.Skip)
            .Take(request.Take)
            .ToList();

        return new PagedResult<T>(items, totalCount, request.PageNumber, request.PageSize);
    }
}