using SharedKernel.Application.Common;
using System.Linq.Expressions;

namespace UserService.Application.DTOs;

public class UserProfileFilterSpecification : BaseSpecification<Domain.Entities.UserProfile>
{
    public UserProfileFilterSpecification(UserProfileFilterDto filter)
    {
        var keyword = filter.Keyword?.Trim().ToLower();

        Criteria = u =>
            (string.IsNullOrEmpty(keyword) ||
             u.FullName.ToLower().Contains(keyword) ||
             (u.PhoneNumber != null && u.PhoneNumber.ToLower().Contains(keyword))) &&
            (!filter.Status.HasValue || u.AccountStatus == filter.Status.Value) &&
            (!filter.Gender.HasValue || u.Gender == filter.Gender.Value) &&
            (!filter.DobFrom.HasValue || u.DayOfBirth >= filter.DobFrom.Value) &&
            (!filter.DobTo.HasValue || u.DayOfBirth <= filter.DobTo.Value);

        // --- ORDER BY ---
        ApplyOrder(filter.OrderBy, filter.Descending);

        // --- PAGING ---
        if (filter is { PageIndex: not null, PageSize: not null })
        {
            var skip = (filter.PageIndex.Value - 1) * filter.PageSize.Value;
            ApplyPaging(skip, filter.PageSize.Value);
        }
    }

    private void ApplyOrder(string? orderBy, bool descending)
    {
        Expression<Func<Domain.Entities.UserProfile, object>> keySelector = orderBy?.ToLower() switch
        {
            "fullname" => u => u.FullName,
            "createdat" => u => u.CreatedAt,
            "gender" => u => u.Gender,
            _ => u => u.CreatedAt
        };

        if (descending) ApplyOrderByDescending(keySelector);
        else ApplyOrderBy(keySelector);
    }

    public static implicit operator UserProfileFilterSpecification(UserProfileFilterDto filter)
        => new(filter);
}