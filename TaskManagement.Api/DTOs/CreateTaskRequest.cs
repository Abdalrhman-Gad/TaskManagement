using TaskManagement.Domain.Enums;

namespace TaskManagement.Api.DTOs;

public class CreateTaskRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Priority Priority { get; set; }
}
