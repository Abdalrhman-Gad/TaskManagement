namespace TaskManagement.Application.Tasks.Commands.UpdateTaskStatus;

using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Application.Tasks.DTOs;
using TaskManagement.Domain.Repositories;

public class UpdateTaskStatusCommandHandler : IRequestHandler<UpdateTaskStatusCommand, TaskDto>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;

    public UpdateTaskStatusCommandHandler(ITaskRepository taskRepository, IUnitOfWork unitOfWork, ICacheService cacheService)
    {
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }

    public async Task<TaskDto> Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.TaskId, cancellationToken);

        if (task == null)
        {
            throw new Exception("Task not found.");
        }

        if (task.UserId != request.UserId)
        {
            throw new Exception("Unauthorized to update this task.");
        }

        task.UpdateStatus(request.NewStatus);
        
        _taskRepository.Update(task);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Invalidate Cache
        await _cacheService.RemoveAsync($"Task_{task.Id}");
        await _cacheService.RemoveAsync($"tasks:user:{request.UserId}");

        return new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            CreatedAt = task.CreatedAt,
            UserId = task.UserId
        };
    }
}
