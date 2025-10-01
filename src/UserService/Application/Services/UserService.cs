using AutoMapper;
using SharedKernel.Application.Common;
using SharedKernel.Domain.Common.Results;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;
using UserService.Application.DTOs;
using UserService.Application.Errors;
using UserService.Application.Services.Interfaces;
using UserService.Domain.Entities;
using UserService.Infrastructure.UnitOfWorks.Interfaces;

namespace UserService.Application.Services;

public class UserService(
    IUserRepository repository,
    ISpecificationRepository<User> specificationRepository,
    IDynamicRepository<User> dynamicRepository,
    IMapper mapper
    ) : IUserService
{
    public async Task<Result<UserDto>> GetByIdAsync(Guid id)
    {
        var user = await repository.GetByIdAsync(id);
        return user is null
            ? UserErrors.NotFound(id)
            : mapper.Map<UserDto>(user);
    }

    public async Task<Result<IReadOnlyList<UserDto>>> GetAllAsync()
    {
        var users = await repository.GetAllAsync();
        var data = mapper.Map<IReadOnlyList<UserDto>>(users);
        return Result.Success(data);
    }

    public async Task<Result> CreateAsync(CreateUserRequest request)
    {
        if (await repository.AnyAsync(x => x.Email.Equals(request.Email)))
            return UserErrors.EmailTaken(request.Email);

        var user = new User(
            request.UserName,
            request.Email,
            BCrypt.Net.BCrypt.HashPassword(request.Password),
            request.PhoneNumber
            );

        await repository.AddAsync(user);
        return true;
    }

    public async Task<Result> UpdateAsync(Guid id, UpdateUserRequest request)
    {
        var user = await repository.GetByIdAsync(id);
        if (user is null)
            return UserErrors.NotFound(id);

        mapper.Map(request, user);
        await repository.Update(user);

        return true;
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        var user = await repository.GetByIdAsync(id);
        if (user is null)
            return UserErrors.NotFound(id);

        await repository.Remove(user);
        return true;
    }

    public async Task<Result<IReadOnlyList<UserDto>>> FilterBySpecification(UserFilterDto filter)
    {
        var spec = new UserFilterSpecification(filter);
        var users = await specificationRepository.ListAsync(spec);
        return Result.Success(mapper.Map<IReadOnlyList<UserDto>>(users));
    }

    public async Task<Result<PagedResult<UserDto>>> FilterByDynamic(DynamicQuery query)
    {
        var result = await dynamicRepository.GetPagedAsync(query);
        var dto = result.Map(mapper.Map<IReadOnlyList<UserDto>>(result.Items));
        return Result.Success(dto);
    }

    public async Task<Result<PagedResult<UserDto>>> FilterPaged(PagedRequest request)
    {
        var result = await repository.GetPagedAsync(request);
        var dto = result.Map(mapper.Map<IReadOnlyList<UserDto>>(result.Items));
        return Result.Success(dto);
    }
}