namespace TaskManagement.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
 
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskManagement.Domain.Repositories;
using TaskManagement.Infrastructure.Persistence;
using Task = TaskManagement.Domain.Entities.Task;

public class TaskRepository : Repository<Task>, ITaskRepository
{
    public TaskRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Task>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Tasks
            .Where(t => t.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasDuplicateTaskAsync(Guid userId, string title, DateTime date, CancellationToken cancellationToken = default)
    {
        return await DbContext.Tasks.AnyAsync(t => 
            t.UserId == userId && 
            t.Title == title && 
            t.CreatedAt.Date == date.Date, cancellationToken);
    }
}