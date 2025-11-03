using AuthService.API.Models;

namespace AuthService.API.DTOs;
public record RegisterRequest(
    string UserName,
    string Email,
    string Password,
    string FullName,
    string? PhoneNumber = null,
    Gender Gender = Gender.Unknown,
    DateTime? DayOfBirth = null,
    string? Address = null,
    string? Avatar = null
);

public record CreateUserProfileInternalRequest(
    Guid UserId,
    string FullName,
    string? PhoneNumber = null,
    Gender Gender = Gender.Unknown,
    DateTime? DayOfBirth = null,
    string? Address = null,
    string? Avatar = null
);

public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginResponse
{
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
    public DateTime ExpiresAt { get; set; }
    public AuthUserResponse User { get; set; } = default!;
}

public class RefreshTokenRequest
{
    public string RefreshToken { get; set; } = default!;
}

public record UserProfileResponse(
    Guid Id,
    string FullName,
    string? PhoneNumber,
    string? Avatar,
    string? Role
);

public class AuthUserResponse
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string AccountStatus { get; set; } = string.Empty;
}

public record RemoteAuthUserRequest(string Role);
public record UpdateStatusAuthUserRequest(string NewStatus);

public record ForgotPasswordRequest(string Email);

public record ResetPasswordRequest(string Token, string NewPassword);

public class UserFilterRequest
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public Role? Role { get; set; }
    public AccountStatus? AccountStatus { get; set; }

    public int? Skip { get; set; }
    public int? Take { get; set; }

    public string? OrderBy { get; set; }
    public bool Descending { get; set; } = false;
}