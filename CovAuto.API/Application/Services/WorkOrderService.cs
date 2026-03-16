using CovAuto.API.Application.DTOs;
using CovAuto.API.Application.Interfaces;
using CovAuto.API.Application.QueryParameters;
using CovAuto.API.Common;
using CovAuto.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CovAuto.API.Application.Services;

/// <summary>
/// Service voor werkorderbeheer inclusief filtering, sorting en pagination.
/// </summary>
public class WorkOrderService : IWorkOrderService
{
    private readonly AppDbContext _context;

    public WorkOrderService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<WorkOrderDto>> GetWorkOrdersAsync(
        WorkOrderQueryParameters queryParams,
        int? teamIdFilter = null)
    {
        // Begin met een basis IQueryable query
        var query = _context.WorkOrders
            .Include(w => w.ServiceTeam)
            .AsQueryable();

        // --- Filtering op team (voor monteurs) ---
        if (teamIdFilter.HasValue)
            query = query.Where(w => w.ServiceTeamId == teamIdFilter.Value);

        // --- Filtering op velden ---
        if (!string.IsNullOrWhiteSpace(queryParams.Title))
            query = query.Where(w => w.Title.Contains(queryParams.Title));

        if (queryParams.MinEstimatedHours.HasValue)
            query = query.Where(w => w.EstimatedHours >= queryParams.MinEstimatedHours.Value);

        if (queryParams.MaxEstimatedHours.HasValue)
            query = query.Where(w => w.EstimatedHours <= queryParams.MaxEstimatedHours.Value);

        if (queryParams.Status.HasValue)
            query = query.Where(w => w.Status == queryParams.Status.Value);

        if (queryParams.Priority.HasValue)
            query = query.Where(w => w.Priority == queryParams.Priority.Value);

        if (!string.IsNullOrWhiteSpace(queryParams.CustomerName))
            query = query.Where(w => w.CustomerName.Contains(queryParams.CustomerName));

        // --- Sorting ---
        query = (queryParams.SortBy.ToLower(), queryParams.SortDirection.ToLower()) switch
        {
            ("title", "asc") => query.OrderBy(w => w.Title),
            ("title", _) => query.OrderByDescending(w => w.Title),
            ("estimatedhours", "asc") => query.OrderBy(w => w.EstimatedHours),
            ("estimatedhours", _) => query.OrderByDescending(w => w.EstimatedHours),
            ("schedulefor", "asc") or ("scheduledfor", "asc") => query.OrderBy(w => w.ScheduledFor),
            ("schedulefor", _) or ("scheduledfor", _) => query.OrderByDescending(w => w.ScheduledFor),
            (_, "asc") => query.OrderBy(w => w.CreatedAt),
            _ => query.OrderByDescending(w => w.CreatedAt) // standaard: createdAt desc
        };

        // --- Pagination: eerst totaal tellen, dan de pagina ophalen ---
        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)queryParams.PageSize);

        var items = await query
            .Skip((queryParams.Page - 1) * queryParams.PageSize)
            .Take(queryParams.PageSize)
            .Select(w => MapToDto(w))
            .ToListAsync();

        return new PagedResult<WorkOrderDto>
        {
            Items = items,
            TotalCount = totalCount,
            TotalPages = totalPages,
            Page = queryParams.Page,
            PageSize = queryParams.PageSize
        };
    }

    public async Task<WorkOrderDto?> GetWorkOrderByIdAsync(int id)
    {
        var workOrder = await _context.WorkOrders
            .Include(w => w.ServiceTeam)
            .FirstOrDefaultAsync(w => w.Id == id);

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

        _context.WorkOrders.Add(workOrder);
        await _context.SaveChangesAsync();

        // Herlaad met navigatie-eigenschap
        await _context.Entry(workOrder).Reference(w => w.ServiceTeam).LoadAsync();

        return MapToDto(workOrder);
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
