using AuthService.API.Models;
using System.ComponentModel.DataAnnotations;

namespace AuthService.API.DTOs;
public record RegisterRequest(
        [Required(ErrorMessage = "Username is required")]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters long")]
        string UserName,

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        string Email,

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        string Password,

        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, ErrorMessage = "Full name cannot exceed 100 characters")]
        string FullName,

        [Phone(ErrorMessage = "Invalid phone number")]
        string? PhoneNumber = null,

        Gender Gender = Gender.Unknown,

        [DataType(DataType.Date)]
        DateTime? DayOfBirth = null,

        [StringLength(255, ErrorMessage = "Address cannot exceed 255 characters")]
        string? Address = null,

        [Url(ErrorMessage = "Invalid avatar URL")]
        string? Avatar = null
    );

public record CreateUserProfileInternalRequest(
    [Required]
        Guid UserId,

    [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, ErrorMessage = "Full name cannot exceed 100 characters")]
        string FullName,

    [Phone(ErrorMessage = "Invalid phone number")]
        string? PhoneNumber = null,

    Gender Gender = Gender.Unknown,

    [DataType(DataType.Date)]
        DateTime? DayOfBirth = null,

    [StringLength(255, ErrorMessage = "Address cannot exceed 255 characters")]
        string? Address = null,

    [Url(ErrorMessage = "Invalid avatar URL")]
        string? Avatar = null
);

public class LoginRequest
{
    [Required(ErrorMessage = "Username is required")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;
}

public class LoginResponse
{
    [Required]
    public string AccessToken { get; set; } = default!;

    [Required]
    public string RefreshToken { get; set; } = default!;

    public DateTime ExpiresAt { get; set; }

    [Required]
    public AuthUserResponse User { get; set; } = default!;
}

public class RefreshTokenRequest
{
    [Required(ErrorMessage = "Refresh token is required")]
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

    [Required]
    public string UserName { get; set; } = string.Empty;

    [Required]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Role { get; set; } = string.Empty;

    [Required]
    public string AccountStatus { get; set; } = string.Empty;
}

public record RemoteAuthUserRequest(
    [Required(ErrorMessage = "Role is required")]
        string Role
);

public record UpdateStatusAuthUserRequest(
    [Required(ErrorMessage = "New status is required")]
        string NewStatus
);

public record ForgotPasswordRequest(
    [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        string Email
);

public record ResetPasswordRequest(
    [Required(ErrorMessage = "Token is required")]
        string Token,

    [Required(ErrorMessage = "New password is required")]
        [MinLength(6, ErrorMessage = "New password must be at least 6 characters long")]
        string NewPassword
);

public class UserFilterRequest
{
    [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters")]
    public string? UserName { get; set; }

    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string? Email { get; set; }

    public Role? Role { get; set; }

    public AccountStatus? AccountStatus { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Skip must be non-negative")]
    public int? Skip { get; set; }

    [Range(1, 1000, ErrorMessage = "Take must be between 1 and 1000")]
    public int? Take { get; set; }

    [StringLength(50, ErrorMessage = "OrderBy cannot exceed 50 characters")]
    public string? OrderBy { get; set; }

    public bool Descending { get; set; } = false;
}