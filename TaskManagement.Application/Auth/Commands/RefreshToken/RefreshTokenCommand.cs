namespace TaskManagement.Application.Auth.Commands.RefreshToken;

using MediatR;
using TaskManagement.Application.Auth.DTOs;

public class RefreshTokenCommand : IRequest<AuthResponse>
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
