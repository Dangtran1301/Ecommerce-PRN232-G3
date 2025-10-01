using SharedKernel.Application.Common;
using SharedKernel.Domain.Common.Results;
using UserService.Application.DTOs;

namespace UserService.Application.Services.Interfaces;

public interface IUserService
{
    Task<Result<UserDto>> GetByIdAsync(Guid id);

    Task<Result<IReadOnlyList<UserDto>>> GetAllAsync();

    Task<Result> CreateAsync(CreateUserRequest request);

    Task<Result> UpdateAsync(Guid id, UpdateUserRequest request);

    Task<Result> DeleteAsync(Guid id);

    Task<Result<IReadOnlyList<UserDto>>> FilterBySpecification(UserFilterDto filter);
    Task<Result<PagedResult<UserDto>>> FilterByDynamic(DynamicQuery query);
    Task<Result<PagedResult<UserDto>>> FilterPaged(PagedRequest request);
}