namespace TaskManagement.Infrastructure.Authentication;

using TaskManagement.Application.Common.Interfaces;
using BCrypt.Net;

public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password)
    {
        return BCrypt.EnhancedHashPassword(password);
    }

    public bool Verify(string password, string hash)
    {
        return BCrypt.EnhancedVerify(password, hash);
    }
}
