namespace TaskManagement.Application.Tasks.Queries.GetAllTasks;

using MediatR;
using System.Collections.Generic;
using TaskManagement.Application.Tasks.DTOs;

public class GetAllTasksQuery : IRequest<IEnumerable<TaskDto>>
{
    public Guid UserId { get; set; }
}

