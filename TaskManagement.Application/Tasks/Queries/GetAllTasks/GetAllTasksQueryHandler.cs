namespace TaskManagement.Application.Tasks.Queries.GetAllTasks;

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Application.Tasks.DTOs;
using TaskManagement.Domain.Repositories;

public class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, IEnumerable<TaskDto>>
{
    private readonly ITaskRepository _taskRepository;
    private readonly ICacheService _cacheService;

    public GetAllTasksQueryHandler(ITaskRepository taskRepository, ICacheService cacheService)
    {
        _taskRepository = taskRepository;
        _cacheService = cacheService;
    }

    public async Task<IEnumerable<TaskDto>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"tasks:user:{request.UserId}";

        var cachedTasks = await _cacheService.GetAsync<IEnumerable<TaskDto>>(cacheKey);
        if (cachedTasks != null)
        {
            return cachedTasks;
        }

        var tasks = await _taskRepository.GetByUserIdAsync(request.UserId, cancellationToken);

        // Sort tasks by Priority first, then by Creation date
        var sortedTasks = tasks
            .OrderBy(t => t.Priority)
            .ThenByDescending(t => t.CreatedAt)
            .Select(task => new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                Priority = task.Priority,
                CreatedAt = task.CreatedAt,
                UserId = task.UserId
            })
            .ToList();

        if (sortedTasks.Any())
        {
            await _cacheService.SetAsync(cacheKey, sortedTasks);
        }

        return sortedTasks;
    }
}
