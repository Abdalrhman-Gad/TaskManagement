namespace TaskManagement.Infrastructure.Services;

 
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Repositories;

public class TaskProcessingBackgroundWorker : BackgroundService
{
    private readonly ITaskProcessingQueue _queue;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<TaskProcessingBackgroundWorker> _logger;

    public TaskProcessingBackgroundWorker(
        ITaskProcessingQueue queue,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<TaskProcessingBackgroundWorker> logger)
    {
        _queue = queue;
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Task Processing Background Worker is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var taskId = await _queue.DequeueTaskAsync(stoppingToken);

                _logger.LogInformation("Processing Task ID: {TaskId}", taskId);

                // Simulate processing time
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var taskRepository = scope.ServiceProvider.GetRequiredService<ITaskRepository>();
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    var taskToProcess = await taskRepository.GetByIdAsync(taskId, stoppingToken);
                    if (taskToProcess != null)
                    {
                        taskToProcess.UpdateStatus(Status.Done);
                        taskRepository.Update(taskToProcess);
                        await unitOfWork.SaveChangesAsync(stoppingToken);
                        
                        _logger.LogInformation("Successfully processed and updated Task ID: {TaskId}", taskId);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Prevent throwing if stoppingToken is canceled
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred executing task processing.");
            }
        }
    }
}
