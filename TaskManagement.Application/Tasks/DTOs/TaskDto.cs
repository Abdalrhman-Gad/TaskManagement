namespace TaskManagement.Application.Tasks.DTOs;

 
using TaskManagement.Domain.Enums;

public class TaskDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Status Status { get; set; }
    public Priority Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid UserId { get; set; }
}
