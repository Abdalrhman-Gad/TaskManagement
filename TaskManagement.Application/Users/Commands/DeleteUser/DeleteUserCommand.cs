namespace TaskManagement.Application.Users.Commands.DeleteUser;

using MediatR;
using System;

public class DeleteUserCommand : IRequest<bool>
{
    public Guid UserId { get; set; }
}
