namespace TaskManagement.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskManagement.Api.DTOs;
using TaskManagement.Application.Auth.Commands.Login;
using TaskManagement.Application.Auth.Commands.Register;
using TaskManagement.Application.Auth.Commands.RefreshToken;

public class AuthController : BaseController
{
    public AuthController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(ApiResponse.Ok(response, "Registered successfully"));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(ApiResponse.Ok(response, "Logged in successfully"));
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(ApiResponse.Ok(response, "Token refreshed successfully"));
    }
}
