using TaskManagement.Domain.Enums;

namespace TaskManagement.Api.DTOs;

public class UpdateTaskStatusRequest
{
    public Status Status { get; set; }
}
