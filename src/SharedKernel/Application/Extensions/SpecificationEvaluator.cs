using Microsoft.EntityFrameworkCore;
using SharedKernel.Application.Interfaces;

namespace SharedKernel.Application.Extensions;

public static class SpecificationEvaluator
{
    public static IQueryable<TEntity> GetQuery<TEntity>(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
        where TEntity : class
    {
        var query = inputQuery;

        if (spec.Criteria != null)
            query = query.Where(spec.Criteria);

        if (spec.Includes?.Any() == true)
            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

        if (spec.OrderBy != null)
            query = query.OrderBy(spec.OrderBy);
        else if (spec.OrderByDescending != null)
            query = query.OrderByDescending(spec.OrderByDescending);

        if (spec.Skip.HasValue)
            query = query.Skip(spec.Skip.Value);
        if (spec.Take.HasValue)
            query = query.Take(spec.Take.Value);

        return query;
    }
}