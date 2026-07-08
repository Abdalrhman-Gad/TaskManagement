namespace TaskManagement.Application.Users.Commands.CreateUser;

using MediatR;
using TaskManagement.Application.Users.DTOs;

public class CreateUserCommand : IRequest<UserDto>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
}
