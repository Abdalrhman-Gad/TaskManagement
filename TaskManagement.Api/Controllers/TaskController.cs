namespace TaskManagement.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskManagement.Api.DTOs;
using TaskManagement.Application.Tasks.Commands.CreateTask;
using TaskManagement.Application.Tasks.Commands.UpdateTaskStatus;
using TaskManagement.Application.Tasks.Queries.GetAllTasks;
using TaskManagement.Application.Tasks.Queries.GetTaskById;

[Authorize]
public class TaskController : BaseController
{
    public TaskController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTasks(CancellationToken cancellationToken)
    {
        var query = new GetAllTasksQuery { UserId = GetCurrentUserId() };
        var tasks = await _mediator.Send(query, cancellationToken);
        var result = tasks.Select(t => new { t.Id, t.Title, t.Description, Status = t.Status.ToString(), t.Priority, t.CreatedAt, t.UserId });
        return Ok(ApiResponse.Ok(result, "Tasks retrieved successfully"));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTaskById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetTaskByIdQuery { TaskId = id, UserId = GetCurrentUserId() };
        var task = await _mediator.Send(query, cancellationToken);
        var result = new { task.Id, task.Title, task.Description, Status = task.Status.ToString(), task.Priority, task.CreatedAt, task.UserId };
        return Ok(ApiResponse.Ok(result, "Task retrieved successfully"));
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateTaskCommand
        {
            UserId = GetCurrentUserId(),
            Title = request.Title,
            Description = request.Description,
            Priority = request.Priority
        };

        var task = await _mediator.Send(command, cancellationToken);
        var result = new { task.Id, task.Title, task.Description, Status = task.Status.ToString(), task.Priority, task.CreatedAt, task.UserId };
        return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, ApiResponse.Ok(result, "Task created successfully", 201));
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateTaskStatus(Guid id, [FromBody] UpdateTaskStatusRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateTaskStatusCommand
        {
            TaskId = id,
            UserId = GetCurrentUserId(),
            NewStatus = request.Status
        };

        var task = await _mediator.Send(command, cancellationToken);
        var result = new { task.Id, task.Title, task.Description, Status = task.Status.ToString(), task.Priority, task.CreatedAt, task.UserId };
        return Ok(ApiResponse.Ok(result, "Task status updated successfully"));
    }
}
