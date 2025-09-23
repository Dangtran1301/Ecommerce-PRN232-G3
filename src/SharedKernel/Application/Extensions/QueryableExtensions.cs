using Microsoft.EntityFrameworkCore;
using SharedKernel.Application.Common;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace SharedKernel.Application.Extensions;

public static class QueryableExtensions
{
    // Safe sorting using reflection -> builds proper Expression<Func<T, TProp>>
    public static IQueryable<T> ApplySorting<T>(this IQueryable<T> source, string? sortBy, bool descending = false)
    {
        if (string.IsNullOrWhiteSpace(sortBy)) return source;

        var prop = typeof(T).GetProperty(sortBy, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        if (prop == null) return source; // or throw new ArgumentException

        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyAccess = Expression.Property(parameter, prop);
        var delegateType = typeof(Func<,>).MakeGenericType(typeof(T), prop.PropertyType);
        var lambda = Expression.Lambda(delegateType, propertyAccess, parameter);

        string methodName = descending ? "OrderByDescending" : "OrderBy";
        var method = typeof(Queryable).GetMethods()
            .First(m => m.Name == methodName && m.GetParameters().Length == 2);
        var generic = method.MakeGenericMethod(typeof(T), prop.PropertyType);

        var result = (IQueryable<T>)generic.Invoke(null, new object[] { source, lambda })!;
        return result;
    }

    // Build where predicate from filter descriptors (basic, covers common types)
    public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> source, IEnumerable<FilterDescriptor> filters)
    {
        var param = Expression.Parameter(typeof(T), "x");
        Expression? combined = null;

        foreach (var f in filters)
        {
            if (string.IsNullOrWhiteSpace(f.Field)) continue;

            var propInfo = typeof(T).GetProperty(f.Field, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (propInfo == null) throw new InvalidOperationException($"Property '{f.Field}' not found on {typeof(T).Name}.");

            var member = Expression.Property(param, propInfo);
            var targetType = Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType;

            // convert value
            object? converted;
            try
            {
                converted = ConvertToType(f.Value, targetType);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Cannot convert filter value for '{f.Field}' to {targetType.Name}: {ex.Message}");
            }

            Expression? exp = null;
            switch (f.Operator)
            {
                case FilterOperator.Equals:
                    exp = Expression.Equal(member, Expression.Constant(converted, propInfo.PropertyType));
                    break;

                case FilterOperator.NotEquals:
                    exp = Expression.Not(Expression.Equal(member, Expression.Constant(converted, propInfo.PropertyType)));
                    break;

                case FilterOperator.GreaterThan:
                    exp = Expression.GreaterThan(member, Expression.Constant(converted, propInfo.PropertyType));
                    break;

                case FilterOperator.LessThan:
                    exp = Expression.LessThan(member, Expression.Constant(converted, propInfo.PropertyType));
                    break;

                case FilterOperator.GreaterOrEqual:
                    exp = Expression.GreaterThanOrEqual(member, Expression.Constant(converted, propInfo.PropertyType));
                    break;

                case FilterOperator.LessOrEqual:
                    exp = Expression.LessThanOrEqual(member, Expression.Constant(converted, propInfo.PropertyType));
                    break;

                case FilterOperator.Contains:
                case FilterOperator.StartsWith:
                case FilterOperator.EndsWith:
                    if (targetType != typeof(string)) throw new InvalidOperationException($"Operator {f.Operator} only valid for string properties.");
                    var methodName = f.Operator == FilterOperator.Contains ? "Contains" : f.Operator == FilterOperator.StartsWith ? "StartsWith" : "EndsWith";
                    var method = typeof(string).GetMethod(methodName, new[] { typeof(string) })!;
                    exp = Expression.Call(member, method, Expression.Constant(converted, typeof(string)));
                    break;

                case FilterOperator.In:
                    // converted expected to be IEnumerable<TElement>
                    if (f.Value is System.Collections.IEnumerable enumerable)
                    {
                        var listType = typeof(List<>).MakeGenericType(targetType);
                        var list = (System.Collections.IList)Activator.CreateInstance(listType)!;
                        foreach (var item in enumerable)
                        {
                            list.Add(ConvertToType(item, targetType)!);
                        }
                        var constList = Expression.Constant(list);
                        var containsMethod = listType.GetMethod("Contains", new[] { targetType })!;
                        exp = Expression.Call(constList, containsMethod, member);
                    }
                    else
                    {
                        throw new InvalidOperationException("In operator expects an array/list value.");
                    }
                    break;

                default:
                    throw new NotSupportedException($"Operator {f.Operator} not supported.");
            }

            combined = combined == null ? exp : Expression.AndAlso(combined, exp);
        }

        if (combined == null) return source;

        var lambda = Expression.Lambda<Func<T, bool>>(combined, param);
        return source.Where(lambda);
    }

    // Convert object -> targetType
    private static object? ConvertToType(object? value, Type targetType)
    {
        if (value == null) return null;

        var dest = Nullable.GetUnderlyingType(targetType) ?? targetType;
        if (dest == typeof(Guid))
        {
            if (value is Guid g) return g;
            return Guid.Parse(value.ToString()!);
        }

        if (dest.IsEnum)
            return Enum.Parse(dest, value.ToString()!, true);

        if (dest == typeof(string))
            return value.ToString();

        if (dest == typeof(DateTime))
            return DateTime.Parse(value.ToString()!);

        // fallback to TypeConverter/Convert.ChangeType
        var converter = TypeDescriptor.GetConverter(dest);
        return converter.ConvertFromInvariantString(value.ToString());
    }

    // ToPagedResultAsync for IQueryable
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(this IQueryable<T> query, DynamicQuery request, CancellationToken cancellationToken = default)
    {
        var total = await query.CountAsync(cancellationToken);
        var items = await query.Skip(request.Skip).Take(request.Take).ToListAsync(cancellationToken);
        return new PagedResult<T>(items, total, request.PageNumber, request.PageSize);
    }
}