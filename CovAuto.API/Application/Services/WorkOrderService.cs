using CovAuto.API.Application.DTOs;
using CovAuto.API.Application.Interfaces;
using CovAuto.API.Application.QueryParameters;
using CovAuto.API.Common;

namespace CovAuto.API.Application.Services;

/// <summary>
/// Service voor werkorderbeheer. Gebruikt de repository voor data-toegang
/// en voert hier de mapping van entity naar DTO uit.
/// </summary>
public class WorkOrderService : IWorkOrderService
{
    private readonly IWorkOrderRepository _workOrderRepository;

    public WorkOrderService(IWorkOrderRepository workOrderRepository)
    {
        _workOrderRepository = workOrderRepository;
    }

    public async Task<PagedResult<WorkOrderDto>> GetWorkOrdersAsync(
        WorkOrderQueryParameters queryParams,
        int? teamIdFilter = null)
    {
        // Haal gepagineerde werkorders op via de repository
        var pagedEntities = await _workOrderRepository.GetPagedAsync(queryParams, teamIdFilter);

        // Zet de entity-resultaten om naar DTOs
        return new PagedResult<WorkOrderDto>
        {
            Items = pagedEntities.Items.Select(MapToDto),
            TotalCount = pagedEntities.TotalCount,
            TotalPages = pagedEntities.TotalPages,
            Page = pagedEntities.Page,
            PageSize = pagedEntities.PageSize
        };
    }

    public async Task<WorkOrderDto?> GetWorkOrderByIdAsync(int id)
    {
        var workOrder = await _workOrderRepository.GetByIdAsync(id);
        return workOrder == null ? null : MapToDto(workOrder);
    }

    public async Task<WorkOrderDto> CreateWorkOrderAsync(CreateWorkOrderRequest request)
    {
        var workOrder = new Domain.Entities.WorkOrder
        {
            Title = request.Title,
            Description = request.Description,
            EstimatedHours = request.EstimatedHours,
            Status = request.Status,
            Priority = request.Priority,
            CreatedAt = DateTime.UtcNow,
            ScheduledFor = request.ScheduledFor,
            CustomerName = request.CustomerName,
            Address = request.Address,
            ServiceTeamId = request.ServiceTeamId
        };

        var saved = await _workOrderRepository.AddAsync(workOrder);
        return MapToDto(saved);
    }

    // Mapping van entity naar DTO
    private static WorkOrderDto MapToDto(Domain.Entities.WorkOrder w) =>
        new()
        {
            Id = w.Id,
            Title = w.Title,
            Description = w.Description,
            EstimatedHours = w.EstimatedHours,
            Status = w.Status.ToString(),
            Priority = w.Priority.ToString(),
            CreatedAt = w.CreatedAt,
            ScheduledFor = w.ScheduledFor,
            CustomerName = w.CustomerName,
            Address = w.Address,
            ServiceTeamId = w.ServiceTeamId,
            ServiceTeamName = w.ServiceTeam?.Name
        };
}
