namespace TaskManagement.Application.Users.Queries.GetCurrentUser;

using MediatR;
using TaskManagement.Application.Users.DTOs;

public class GetCurrentUserQuery : IRequest<UserDto>
{
    public string Email { get; set; }
}
