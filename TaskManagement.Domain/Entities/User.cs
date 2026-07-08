namespace TaskManagement.Domain.Entities;

using TaskManagement.Domain.Common.Models;
using System.Collections.Generic;
using System.Linq;

public class User : AggregateRoot
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Password { get; private set; } = string.Empty;
    public bool IsDeleted { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    public string? RefreshToken { get; private set; }
    public DateTime? RefreshTokenExpiryTime { get; private set; }

    public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();
    private readonly List<UserRole> _userRoles = new();

    private User() { }

    private User(Guid id, string name, string email, string password, DateTime createdAt) : base(id)
    {
        Name = name;
        Email = email;
        Password = password;
        CreatedAt = createdAt;
        IsDeleted = false;
    }

    public static User Create(string name, string email, string password)
    {
        return new User(Guid.NewGuid(), name, email, password, DateTime.UtcNow);
    }
    
    public void AddRole(Role role)
    {
        if (!_userRoles.Any(ur => ur.RoleId == role.Id))
        {
            _userRoles.Add(new UserRole { UserId = Id, RoleId = role.Id });
        }
    }
    
    public void UpdateRefreshToken(string refreshToken, DateTime expiryTime)
    {
        RefreshToken = refreshToken;
        RefreshTokenExpiryTime = expiryTime;
    }

    public void RevokeRefreshToken()
    {
        RefreshToken = null;
        RefreshTokenExpiryTime = null;
    }

    public void Delete()
    {
        IsDeleted = true;
    }
}
