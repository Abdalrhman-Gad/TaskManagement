namespace TaskManagement.Application.Common.Interfaces;

using TaskManagement.Domain.Entities;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
    string GenerateRefreshToken();
}
