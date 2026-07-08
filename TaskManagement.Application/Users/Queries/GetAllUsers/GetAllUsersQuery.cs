namespace TaskManagement.Application.Users.Queries.GetAllUsers;

using MediatR;
using System.Collections.Generic;
using TaskManagement.Application.Users.DTOs;

public class GetAllUsersQuery : IRequest<List<UserDto>>
{
}
