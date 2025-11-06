using System.Linq.Expressions;
using AuthService.API.Models;
using SharedKernel.Application.Common;

namespace AuthService.API.DTOs;

public class UserSpecification : BaseSpecification<User>
{
    public UserSpecification(UserFilterRequest filter)
    {
        var keywordUserName = filter.UserName?.Trim();
        var keywordEmail = filter.Email?.Trim();

        Criteria = u =>
            (string.IsNullOrEmpty(keywordUserName) || u.UserName.Contains(keywordUserName)) &&
            (string.IsNullOrEmpty(keywordEmail) || u.Email.Contains(keywordEmail)) &&
            (!filter.Role.HasValue || u.Role == filter.Role.Value) &&
            (!filter.AccountStatus.HasValue || u.AccountStatus == filter.AccountStatus.Value);

        // --- ORDER BY ---
        ApplyOrder(filter.OrderBy, filter.Descending);

        // --- PAGING ---
        if (filter is { Skip: not null, Take: not null })
            ApplyPaging(filter.Skip.Value, filter.Take.Value);
    }

    private void ApplyOrder(string? orderBy, bool descending)
    {
        Expression<Func<User, object>> keySelector = orderBy?.ToLower() switch
        {
            "username" => u => u.UserName,
            "email" => u => u.Email,
            "role" => u => u.Role,
            "createdat" => u => u.CreatedAt,
            _ => u => u.CreatedAt
        };

        if (descending)
            ApplyOrderByDescending(keySelector);
        else
            ApplyOrderBy(keySelector);
    }

    public static implicit operator UserSpecification(UserFilterRequest filter)
        => new(filter);
}