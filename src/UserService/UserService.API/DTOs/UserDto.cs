namespace UserService.API.DTOs;

public record CreateUserRequest(string UserName, string Email, string Password, string? PhoneNumber);
public record UserResponse(Guid Id, string UserName, string Email, string? PhoneNumber, bool IsActive, bool EmailConfirmed);