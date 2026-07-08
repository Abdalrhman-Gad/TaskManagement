namespace TaskManagement.Application.Auth.Commands.Register;

using MediatR;
using TaskManagement.Application.Auth.DTOs;

public class RegisterCommand : IRequest<AuthResponse>
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
