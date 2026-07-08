namespace TaskManagement.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using TaskManagement.Api.DTOs;
using TaskManagement.Application.Users.Commands.CreateUser;
using TaskManagement.Application.Users.Commands.DeleteUser;
using TaskManagement.Application.Users.Queries.GetAllUsers;
using TaskManagement.Application.Users.Queries.GetCurrentUser;

[Authorize]
public class UserController : BaseController
{
    public UserController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUserProfile(CancellationToken cancellationToken)
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(email))
        {
            return Unauthorized(ApiResponse.Error("Unauthorized access.", 401));
        }

        try
        {
            var user = await _mediator.Send(new GetCurrentUserQuery { Email = email }, cancellationToken);
            return Ok(ApiResponse.Ok(user, "Profile retrieved successfully"));
        }
        catch (Exception ex)
        {
            return NotFound(ApiResponse.Error(ex.Message, 404));
        }
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
    {
        var users = await _mediator.Send(new GetAllUsersQuery(), cancellationToken);
        return Ok(ApiResponse.Ok(users, "Users retrieved successfully"));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var command = new CreateUserCommand 
            { 
                Name = request.Name, 
                Email = request.Email, 
                Password = request.Password, 
                Role = request.Role 
            };
            var user = await _mediator.Send(command, cancellationToken);
            return Ok(ApiResponse.Ok(user, "User created successfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse.Error(ex.Message));
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _mediator.Send(new DeleteUserCommand { UserId = id }, cancellationToken);
            return Ok(ApiResponse.Ok(message: "User deleted successfully"));
        }
        catch (Exception ex)
        {
            return NotFound(ApiResponse.Error(ex.Message, 404));
        }
    }
}
