using System.ComponentModel.DataAnnotations;
using UserService.Domain.Entities.Enums;

namespace UserService.Application.DTOs;

public record CreateUserRequest(
    [Required(ErrorMessage = "FullName is required")]
    [StringLength(100, ErrorMessage = "FullName cannot exceed 100 characters")]
    string FullName,

    [Required(ErrorMessage = "UserName is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "UserName must be between 3 and 50 characters")]
    string UserName,

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    string Email,

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    string Password,

    [Phone(ErrorMessage = "Invalid phone number format")]
    string? PhoneNumber
);

public record UserDto(
    Guid Id,
    string FullName,
    string UserName,
    string Email,
    string? PhoneNumber,
    string? Avatar);

public record UpdateUserRequest(
    [StringLength(100, ErrorMessage = "FullName cannot exceed 100 characters")]
    string? FullName,

    [Phone(ErrorMessage = "Invalid phone number format")]
    string? PhoneNumber,

    [StringLength(255, ErrorMessage = "Avatar URL cannot exceed 255 characters")]
    string? Avatar
);

public class UserFilterDto
{
    public string? Keyword { get; set; }
    public Role? Role { get; set; }
    public AccountStatus? Status { get; set; }
    public Gender? Gender { get; set; }
    public DateTime? DobFrom { get; set; }
    public DateTime? DobTo { get; set; }
    public int? PageIndex { get; set; } = 1;
    public int? PageSize { get; set; } = 25;
    public string? OrderBy { get; set; } = "CreatedAt";
    public bool Descending { get; set; } = false;
}