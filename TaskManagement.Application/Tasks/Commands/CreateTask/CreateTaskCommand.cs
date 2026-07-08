namespace TaskManagement.Application.Tasks.Commands.CreateTask;

using MediatR;
using TaskManagement.Application.Tasks.DTOs;
using TaskManagement.Domain.Enums;

public class CreateTaskCommand : IRequest<TaskDto>
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Priority Priority { get; set; }
}

