using CovAuto.API.Domain.Enums;

namespace CovAuto.API.Domain.Entities;

public class WorkOrder
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double EstimatedHours { get; set; }
    public WorkOrderStatus Status { get; set; }
    public WorkOrderPriority Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ScheduledFor { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int ServiceTeamId { get; set; }

    // Navigation property
    public ServiceTeam? ServiceTeam { get; set; }
}