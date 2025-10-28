namespace AuthService.API.DTOs;

public class LoginRequestDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginResponseDto
{
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
    public DateTime ExpiresAt { get; set; }
    public AuthUserDto User { get; set; } = default!;
}

public class RefreshTokenRequestDto
{
    public string RefreshToken { get; set; } = default!;
}

public record UserServiceUserDto(
    Guid Id,
    string FullName,
    string UserName,
    string Email,
    string? PhoneNumber,
    string? Avatar,
    string? Role
);

public class AuthUserDto
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}