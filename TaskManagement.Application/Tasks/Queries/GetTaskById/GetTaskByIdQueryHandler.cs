namespace TaskManagement.Application.Tasks.Queries.GetTaskById;

using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Application.Tasks.DTOs;
using TaskManagement.Domain.Repositories;

public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskDto>
{
    private readonly ITaskRepository _taskRepository;
    private readonly ICacheService _cache;

    public GetTaskByIdQueryHandler(ITaskRepository taskRepository, ICacheService cache)
    {
        _taskRepository = taskRepository;
        _cache = cache;
    }

    public async Task<TaskDto> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"Task_{request.TaskId}";
        
        var taskDto = await _cache.GetAsync<TaskDto>(cacheKey);

        if (taskDto == null)
        {
            var task = await _taskRepository.GetByIdAsync(request.TaskId, cancellationToken);

            if (task == null)
            {
                throw new Exception("Task not found.");
            }

            taskDto = new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                Priority = task.Priority,
                CreatedAt = task.CreatedAt,
                UserId = task.UserId
            };

            await _cache.SetAsync(cacheKey, taskDto, TimeSpan.FromMinutes(10));
        }

        if (taskDto.UserId != request.UserId)
        {
            throw new Exception("Unauthorized to view this task.");
        }

        return taskDto;
    }
}
