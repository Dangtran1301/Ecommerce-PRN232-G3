namespace UserService.Application.DTOs;

public record CreateUserRequest(
    string FullName,
    string UserName,
    string Email,
    string Password,
    string? PhoneNumber);

public record UserDto(
    Guid Id,
    string FullName,
    string UserName,
    string Email,
    string? PhoneNumber,
    string? Avatar);

public record UpdateUserRequest(
    string? FullName,
    string? PhoneNumber,
    string? Avatar);