namespace TaskManagement.Application.Common.Behaviors;

using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        _logger.LogInformation("TaskManagement Request Started: {Name} {@Request}", requestName, request);

        var timer = new Stopwatch();
        timer.Start();

        var response = await next();

        timer.Stop();

        if (timer.ElapsedMilliseconds > 500)
        {
            _logger.LogWarning("TaskManagement Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@Request}",
                requestName, timer.ElapsedMilliseconds, request);
        }

        _logger.LogInformation("TaskManagement Request Completed: {Name} {@Response}", requestName, response);

        return response;
    }
}
