namespace TaskManagement.Application.Tasks.Commands.UpdateTaskStatus;

using MediatR;
 
using TaskManagement.Application.Tasks.DTOs;
using TaskManagement.Domain.Enums;

public class UpdateTaskStatusCommand : IRequest<TaskDto>
{
    public Guid TaskId { get; set; }
    public Guid UserId { get; set; }
    public Status NewStatus { get; set; }
}

