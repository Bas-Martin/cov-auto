using CovAuto.API.Domain.Enums;

namespace CovAuto.API.Application.QueryParameters;

/// <summary>
/// Query parameters voor het filteren, sorteren en pagineren van werkorders.
/// </summary>
public class WorkOrderQueryParameters
{
    // --- Filtering ---
    public string? Title { get; set; }
    public double? MinEstimatedHours { get; set; }
    public double? MaxEstimatedHours { get; set; }
    public WorkOrderStatus? Status { get; set; }
    public WorkOrderPriority? Priority { get; set; }
    public string? CustomerName { get; set; }

    // --- Sorting ---
    // Mogelijke waarden: title, estimatedHours, createdAt, scheduledFor
    public string SortBy { get; set; } = "createdAt";
    // asc of desc
    public string SortDirection { get; set; } = "desc";

    // --- Pagination ---
    private int _page = 1;
    public int Page
    {
        get => _page;
        set => _page = value < 1 ? 1 : value;
    }

    private int _pageSize = 10;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value < 1 ? 1 : (value > 100 ? 100 : value);
    }
}
