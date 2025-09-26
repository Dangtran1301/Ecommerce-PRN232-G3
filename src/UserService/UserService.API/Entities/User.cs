using SharedKernel.Domain.Common.Entities;
using UserService.API.Entities.Enums;

namespace UserService.API.Entities;

public class User : AuditableEntity<Guid>
{
    public string FullName { get; set; } = string.Empty;

    public string UserName { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    //public bool EmailConfirmed { get; private set; } = false;
    public string? PhoneNumber { get; private set; } = string.Empty;

    public Gender Gender { get; set; } = Gender.Unknown;

    public DateTime? DayOfBirth { get; set; } = default!;

    public string? Address = string.Empty;
    public string? Avatar { get; set; } = string.Empty;
    public AccountStatus AccountStatus { get; set; } = AccountStatus.Active;
    public Role Role { get; set; } = Role.Customer;
    public ICollection<UserSession> AccountTokens { get; set; } = [];

    protected User()
    { }

    public User(string userName, string email, string passwordHash, string? phoneNumber = null)
    {
        Id = Guid.NewGuid();
        UserName = userName;
        Email = email;
        PasswordHash = passwordHash;
        PhoneNumber = phoneNumber;
        SetCreated("system");
    }

    //public void ConfirmEmail() => EmailConfirmed = true;
    public void ChangePassword(string newHash) => PasswordHash = newHash;
}