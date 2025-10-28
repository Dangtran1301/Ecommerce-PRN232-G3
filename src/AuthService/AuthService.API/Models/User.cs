using SharedKernel.Domain.Common.Entities;

namespace AuthService.API.Models;

public class User : AuditableEntity<Guid>
{
    public string UserName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string? PhoneNumber { get; private set; }
    public bool EmailConfirmed { get; private set; } = false;
    public bool IsActive { get; private set; } = true;
    public Role Role { get; private set; } = Role.Customer;

    protected User() { }

    public User(string userName, string email, string passwordHash, string? phoneNumber = null)
    {
        Id = Guid.NewGuid();
        UserName = userName;
        Email = email;
        PasswordHash = passwordHash;
        PhoneNumber = phoneNumber;
        SetCreated("system");
    }

    public void ChangePassword(string newHash)
        => PasswordHash = newHash;

    public void ConfirmEmail()
        => EmailConfirmed = true;

    public void Activate()
        => IsActive = true;

    public void Deactivate()
        => IsActive = false;

    public void ChangeRole(Role role)
        => Role = role;
}

public enum Role
{
    Admin,
    Seller,
    Customer
}