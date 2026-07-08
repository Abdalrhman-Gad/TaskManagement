namespace TaskManagement.Application.Common.Interfaces;

 
using System.Threading;
using System.Threading.Tasks;

public interface ITaskProcessingQueue
{
    ValueTask QueueTaskAsync(Guid taskId, CancellationToken cancellationToken = default);
    ValueTask<Guid> DequeueTaskAsync(CancellationToken cancellationToken = default);
}
