namespace TaskManagement.Application.Tasks.Commands.CreateTask;

using MediatR;
 
using System.Threading;
using System.Threading.Tasks;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Application.Tasks.DTOs;
using TaskManagement.Domain.Repositories;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskDto>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITaskProcessingQueue _queue;
    private readonly ICacheService _cacheService;

    public CreateTaskCommandHandler(
        ITaskRepository taskRepository, 
        IUnitOfWork unitOfWork,
        ITaskProcessingQueue queue,
        ICacheService cacheService)
    {
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
        _queue = queue;
        _cacheService = cacheService;
    }

    public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        if (await _taskRepository.HasDuplicateTaskAsync(request.UserId, request.Title, DateTime.UtcNow, cancellationToken))
        {
            throw new Exception("Duplicate task with the same title on the same day is not allowed.");
        }

        var task = Domain.Entities.Task.Create(request.Title, request.Description, request.Priority, request.UserId);

        await _taskRepository.AddAsync(task, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _queue.QueueTaskAsync(task.Id, cancellationToken);

        // Invalidate the tasks query cache
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
