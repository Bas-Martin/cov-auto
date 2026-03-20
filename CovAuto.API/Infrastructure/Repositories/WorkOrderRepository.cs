using CovAuto.API.Application.Interfaces;
using CovAuto.API.Application.QueryParameters;
using CovAuto.API.Common;
using CovAuto.API.Domain.Entities;
using CovAuto.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CovAuto.API.Infrastructure.Repositories;

/// <summary>
/// Repository voor werkorders. Handelt alle database-queries voor WorkOrder af,
/// inclusief filtering, sorting en pagination.
/// </summary>
public class WorkOrderRepository : IWorkOrderRepository
{
    private readonly AppDbContext _context;

    public WorkOrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<WorkOrder>> GetPagedAsync(
        WorkOrderQueryParameters queryParams,
        int? teamIdFilter = null)
    {
        // Begin met een basis query inclusief navigatie-eigenschap
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
            .ToListAsync();

        return new PagedResult<WorkOrder>
        {
            Items = items,
            TotalCount = totalCount,
            TotalPages = totalPages,
            Page = queryParams.Page,
            PageSize = queryParams.PageSize
        };
    }

    public async Task<WorkOrder?> GetByIdAsync(int id)
    {
        return await _context.WorkOrders
            .Include(w => w.ServiceTeam)
            .FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<WorkOrder> AddAsync(WorkOrder workOrder)
    {
        _context.WorkOrders.Add(workOrder);
        await _context.SaveChangesAsync();

        // Herlaad met navigatie-eigenschap
        await _context.Entry(workOrder).Reference(w => w.ServiceTeam).LoadAsync();

        return workOrder;
    }

    public async Task<List<WorkOrder>> GetByTeamAndPeriodAsync(int teamId, DateTime from, DateTime to)
    {
        return await _context.WorkOrders
            .Where(w => w.ServiceTeamId == teamId && w.CreatedAt >= from && w.CreatedAt <= to)
            .ToListAsync();
    }
}
