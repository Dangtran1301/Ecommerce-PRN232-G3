using System.ComponentModel.DataAnnotations;
using UserService.Domain.Entities.Enums;

namespace UserService.Application.DTOs;

public record CreateUserProfileRequest(
    [Required(ErrorMessage = "Need user id to create user profile")]
    Guid UserId,
    [Required(ErrorMessage = "FullName is required")]
    [StringLength(100, ErrorMessage = "FullName cannot exceed 100 characters")]
    [RegularExpression(@"^[\p{L}\s]+$", ErrorMessage = "FullName only allows letters and spaces")]
    string FullName,

    [Phone(ErrorMessage = "Invalid phone number format")]
    string? PhoneNumber,

    Gender Gender = Gender.Unknown,

    DateTime? DayOfBirth = null,

    string? Address = null,

    string? Avatar = null
);

public record CreateUserProfileFromAuthRequest(
    Guid UserId,
    string FullName,
    string? PhoneNumber = null,
    Gender Gender = Gender.Unknown,
    DateTime? DayOfBirth = null,
    string? Address = null,
    string? Avatar = null
);

public record UserProfileDto(
    Guid Id,
    string FullName,
    string? PhoneNumber,
    string? Avatar,
    Gender Gender,
    DateTime? DayOfBirth,
    string? Address
);

public record UpdateUserProfileRequest(
    [StringLength(100, ErrorMessage = "FullName cannot exceed 100 characters")]
    string? FullName,

    [Phone(ErrorMessage = "Invalid phone number format")]
    string? PhoneNumber,

    [StringLength(255, ErrorMessage = "Avatar URL cannot exceed 255 characters")]
    string? Avatar,

    Gender? Gender = null,

    DateTime? DayOfBirth = null,

    string? Address = null
);

public class UserProfileFilterDto
{
    public string? Keyword { get; set; }
    public Gender? Gender { get; set; }
    public DateTime? DobFrom { get; set; }
    public DateTime? DobTo { get; set; }
    public int? PageIndex { get; set; } = 1;
    public int? PageSize { get; set; } = 25;
    public string? OrderBy { get; set; } = "CreatedAt";
    public bool Descending { get; set; } = false;
}