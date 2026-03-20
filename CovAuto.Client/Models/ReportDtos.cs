namespace CovAuto.Client.Models;

public class TeamReportRequest
{
    public DateTime From { get; set; } = DateTime.UtcNow.AddMonths(-1);
    public DateTime To { get; set; } = DateTime.UtcNow;
}

public class BulkReportRequest
{
    public List<int> TeamIds { get; set; } = new();
    public DateTime From { get; set; } = DateTime.UtcNow.AddMonths(-1);
    public DateTime To { get; set; } = DateTime.UtcNow;
}

public class TeamReportDto
{
    public int TeamId { get; set; }
    public string TeamName { get; set; } = string.Empty;
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public int TotalWorkOrders { get; set; }
    public double TotalEstimatedHours { get; set; }
    public Dictionary<string, int> WorkOrdersByStatus { get; set; } = new();
    public Dictionary<string, int> WorkOrdersByPriority { get; set; } = new();
    public string GeneratedContent { get; set; } = string.Empty;
    public DateTime GeneratedAt { get; set; }
}

public class PerformanceComparisonDto
{
    public long SequentialDurationMs { get; set; }
    public long ParallelDurationMs { get; set; }
    public long DifferenceMs { get; set; }
    public string Summary { get; set; } = string.Empty;
}
