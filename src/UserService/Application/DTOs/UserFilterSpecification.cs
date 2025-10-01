using SharedKernel.Application.Common;
using UserService.Domain.Entities;

namespace UserService.Application.DTOs;

public class UserFilterSpecification : BaseSpecification<User>
{
    public UserFilterSpecification(UserFilterDto filter)
    {
        Criteria = u =>
            (string.IsNullOrEmpty(filter.Keyword) ||
             u.UserName.Contains(filter.Keyword) ||
             u.FullName.Contains(filter.Keyword) ||
             u.Email.Contains(filter.Keyword)) &&

            (!filter.Role.HasValue || u.Role == filter.Role.Value) &&
            (!filter.Status.HasValue || u.AccountStatus == filter.Status.Value) &&
            (!filter.Gender.HasValue || u.Gender == filter.Gender.Value) &&
            (!filter.DobFrom.HasValue || u.DayOfBirth >= filter.DobFrom.Value) &&
            (!filter.DobTo.HasValue || u.DayOfBirth <= filter.DobTo.Value);


        if (!string.IsNullOrEmpty(filter.OrderBy))
        {
            switch (filter.OrderBy.ToLower())
            {
                case "username":
                    if (filter.Descending) ApplyOrderByDescending(u => u.UserName);
                    else ApplyOrderBy(u => u.UserName);
                    break;

                case "fullname":
                    if (filter.Descending) ApplyOrderByDescending(u => u.FullName);
                    else ApplyOrderBy(u => u.FullName);
                    break;

                case "email":
                    if (filter.Descending) ApplyOrderByDescending(u => u.Email);
                    else ApplyOrderBy(u => u.Email);
                    break;

                default:
                    if (filter.Descending) ApplyOrderByDescending(u => u.CreatedAt);
                    else ApplyOrderBy(u => u.CreatedAt);
                    break;
            }
        }

        if (filter is not { PageIndex: not null, PageSize: not null }) return;
        var skip = (filter.PageIndex.Value - 1) * filter.PageSize.Value;
        var take = filter.PageSize.Value;
        ApplyPaging(skip, take);
    }
    public static implicit operator UserFilterSpecification(UserFilterDto filter)
        => new UserFilterSpecification(filter);
}