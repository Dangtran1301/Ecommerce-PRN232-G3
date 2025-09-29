using AutoMapper;
using SharedKernel.Domain.Common.Results;
using UserService.Application.DTOs;
using UserService.Application.Errors;
using UserService.Application.Services.Interfaces;
using UserService.Domain.Entities;
using UserService.Infrastructure.UnitOfWorks.Interfaces;

namespace UserService.Application.Services;

public class UserService(IUserRepository repository, IMapper mapper) : IUserService
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

    public async Task<Result<UserDto>> CreateAsync(CreateUserRequest request)
    {
        var existing = await repository.GetByEmailAsync(request.Email);
        if (existing is not null)
            return UserErrors.EmailTaken(request.Email);

        var user = new User(request.UserName, request.Email, BCrypt.Net.BCrypt.HashPassword(request.Password),
            request.PhoneNumber);

        await repository.AddAsync(user);
        return Result.Success(mapper.Map<UserDto>(user));
    }

    public async Task<Result<UserDto>> UpdateAsync(Guid id, UpdateUserRequest request)
    {
        var user = await repository.GetByIdAsync(id);
        if (user is null)
            return UserErrors.NotFound(id);

        mapper.Map(request, user);
        repository.Update(user);

        return Result.Success(mapper.Map<UserDto>(user));
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        var user = await repository.GetByIdAsync(id);
        if (user is null)
            return UserErrors.NotFound(id);

        repository.Remove(user);
        return Result<object>.Success(id);
    }
}