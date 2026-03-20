using CovAuto.API.Domain.Enums;

namespace CovAuto.API.Application.DTOs;

public class WorkOrderDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double EstimatedHours { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ScheduledFor { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int ServiceTeamId { get; set; }
    public string? ServiceTeamName { get; set; }
}
