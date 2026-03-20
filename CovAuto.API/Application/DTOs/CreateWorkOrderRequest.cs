using System.ComponentModel.DataAnnotations;
using CovAuto.API.Domain.Enums;

namespace CovAuto.API.Application.DTOs;

public class CreateWorkOrderRequest
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Range(0.5, 999)]
    public double EstimatedHours { get; set; }

    public WorkOrderStatus Status { get; set; } = WorkOrderStatus.Nieuw;

    public WorkOrderPriority Priority { get; set; } = WorkOrderPriority.Normaal;

    public DateTime? ScheduledFor { get; set; }

    [Required]
    [MaxLength(200)]
    public string CustomerName { get; set; } = string.Empty;

    [Required]
    [MaxLength(300)]
    public string Address { get; set; } = string.Empty;

    [Required]
    public int ServiceTeamId { get; set; }
}