namespace CovAuto.Client.Models;

public class WorkOrderQueryParameters
{
    public string? Title { get; set; }
    public string? Status { get; set; }
    public string? Priority { get; set; }
    public string? CustomerName { get; set; }
    public double? MinEstimatedHours { get; set; }
    public double? MaxEstimatedHours { get; set; }
    public string SortBy { get; set; } = "createdAt";
    public string SortDirection { get; set; } = "desc";
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public string ToQueryString()
    {
        var parts = new List<string>();
        if (!string.IsNullOrEmpty(Title)) parts.Add($"title={Uri.EscapeDataString(Title)}");
        if (!string.IsNullOrEmpty(Status)) parts.Add($"status={Uri.EscapeDataString(Status)}");
        if (!string.IsNullOrEmpty(Priority)) parts.Add($"priority={Uri.EscapeDataString(Priority)}");
        if (!string.IsNullOrEmpty(CustomerName)) parts.Add($"customerName={Uri.EscapeDataString(CustomerName)}");
        if (MinEstimatedHours.HasValue) parts.Add($"minEstimatedHours={MinEstimatedHours}");
        if (MaxEstimatedHours.HasValue) parts.Add($"maxEstimatedHours={MaxEstimatedHours}");
        parts.Add($"sortBy={SortBy}");
        parts.Add($"sortDirection={SortDirection}");
        parts.Add($"page={Page}");
        parts.Add($"pageSize={PageSize}");
        return string.Join("&", parts);
    }
}
