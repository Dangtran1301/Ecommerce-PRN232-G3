using SharedKernel.Domain.Common.Entities;
using System.Security.Cryptography;

namespace AuthService.API.Models;

public class User : AuditableEntity<Guid>
{
    public string UserName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public Role Role { get; private set; } = Role.Customer;
    public AccountStatus AccountStatus { get; set; } = AccountStatus.Active;
    public string? ResetToken { get; private set; }
    public DateTime? ResetTokenExpiry { get; private set; }

    protected User()
    { }

    public User(string userName, string email, string passwordHash, Role role = Role.Customer)
    {
        Id = Guid.NewGuid();
        UserName = userName;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        SetCreated("system");
    }

    public void ChangePassword(string newHash)
        => PasswordHash = newHash;

    public void Activate()
        => AccountStatus = AccountStatus.Active;

    public void Deactivate()
        => AccountStatus = AccountStatus.Inactive;

    public void ChangeRole(Role role)
        => Role = role;

    public void GenerateResetToken()
    {
        var tokenBytes = RandomNumberGenerator.GetBytes(64);
        ResetToken = Convert.ToBase64String(tokenBytes);
        ResetTokenExpiry = DateTime.UtcNow.AddMinutes(15);
    }

    public void ClearResetToken()
    {
        ResetToken = null;
        ResetTokenExpiry = null;
    }

    public bool IsResetTokenValid(string token)
        => ResetToken == token && ResetTokenExpiry > DateTime.UtcNow;
}

public enum Role
{
    Admin,
    Seller,
    Customer
}

public enum AccountStatus
{
    Active,
    Inactive,
}

public enum Gender
{
    Male = 0,
    Female = 1,
    Unknown = 2,
}