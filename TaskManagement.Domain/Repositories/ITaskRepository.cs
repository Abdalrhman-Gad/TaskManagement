namespace TaskManagement.Domain.Repositories;

 
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaskManagement.Domain.Entities;
using Task = TaskManagement.Domain.Entities.Task;

public interface ITaskRepository : IRepository<Task>
{
    Task<IEnumerable<Task>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> HasDuplicateTaskAsync(Guid userId, string title, DateTime date, CancellationToken cancellationToken = default);
}
