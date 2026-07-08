namespace TaskManagement.Domain.Entities;

using TaskManagement.Domain.Common.Models;
using TaskManagement.Domain.Enums;

public class Task : AggregateRoot
{
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Status Status { get; private set; }
    public Priority Priority { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Guid UserId { get; private set; }

    private Task() { }

    private Task(Guid id, string title, string description, Priority priority, Guid userId) : base(id)
    {
        Title = title;
        Description = description;
        Status = Status.Todo;
        Priority = priority;
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
    }

    public static Task Create(string title, string description, Priority priority, Guid userId)
    {
        return new Task(Guid.NewGuid(), title, description, priority, userId);
    }

    public void UpdateStatus(Status status)
    {
        Status = status;
    }
}
