namespace TaskManagement.Application.Tasks.Queries.GetTaskById;

 
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using TaskManagement.Application.Tasks.DTOs;
using TaskManagement.Domain.Repositories;

public class GetTaskByIdQuery : IRequest<TaskDto>
{
    public Guid TaskId { get; set; }
    public Guid UserId { get; set; }
}

