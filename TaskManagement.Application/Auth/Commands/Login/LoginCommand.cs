namespace TaskManagement.Application.Auth.Commands.Login;

using MediatR;
using TaskManagement.Application.Auth.DTOs;

public class LoginCommand : IRequest<AuthResponse>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
