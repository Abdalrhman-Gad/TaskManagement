namespace TaskManagement.Infrastructure.Services;

 
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using TaskManagement.Application.Common.Interfaces;

public class TaskProcessingQueue : ITaskProcessingQueue
{
    private readonly Channel<Guid> _queue;

    public TaskProcessingQueue()
    {
        var options = new BoundedChannelOptions(100)
        {
            FullMode = BoundedChannelFullMode.Wait
        };
        _queue = Channel.CreateBounded<Guid>(options);
    }

    public async ValueTask QueueTaskAsync(Guid taskId, CancellationToken cancellationToken = default)
    {
        await _queue.Writer.WriteAsync(taskId, cancellationToken);
    }

    public async ValueTask<Guid> DequeueTaskAsync(CancellationToken cancellationToken = default)
    {
        return await _queue.Reader.ReadAsync(cancellationToken);
    }
}
