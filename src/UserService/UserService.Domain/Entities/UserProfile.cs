using SharedKernel.Domain.Common.Entities;
using UserService.Domain.Entities.Enums;

namespace UserService.Domain.Entities;

public class UserProfile : AuditableEntity<Guid>
{
    public string FullName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public Gender Gender { get; set; } = Gender.Unknown;
    public DateTime? DayOfBirth { get; set; }
    public string? Address { get; set; }
    public string? Avatar { get; set; }

    protected UserProfile()
    { }

    public UserProfile(Guid userId, string fullName, string? phoneNumber = null)
    {
        Id = userId;
        FullName = fullName;
        PhoneNumber = phoneNumber;
        SetCreated("system");
    }

    public void ChangePhone(string? newPhone) => PhoneNumber = newPhone;

    public void ChangeAddress(string? newAddress) => Address = newAddress;

    public void ChangeAvatar(string? newAvatar) => Avatar = newAvatar;

    public void ChangeName(string newName) => FullName = newName;
}