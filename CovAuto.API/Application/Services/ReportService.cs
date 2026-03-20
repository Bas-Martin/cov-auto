using System.Diagnostics;
using System.Text;
using CovAuto.API.Application.DTOs;
using CovAuto.API.Application.Interfaces;

namespace CovAuto.API.Application.Services;

/// <summary>
/// Service voor het genereren van rapporten per team en periode.
/// Demonstreert het verschil tussen sequentiële en parallelle verwerking.
/// </summary>
public class ReportService : IReportService
{
    private readonly IServiceTeamRepository _teamRepository;
    private readonly IWorkOrderRepository _workOrderRepository;
    private readonly ILogger<ReportService> _logger;

    public ReportService(
        IServiceTeamRepository teamRepository,
        IWorkOrderRepository workOrderRepository,
        ILogger<ReportService> logger)
    {
        _teamRepository = teamRepository;
        _workOrderRepository = workOrderRepository;
        _logger = logger;
    }

    /// <summary>
    /// Genereert een rapport voor één team over een bepaalde periode.
    /// </summary>
    public async Task<TeamReportDto> GenerateTeamReportAsync(int teamId, DateTime from, DateTime to)
    {
        _logger.LogInformation("Rapport genereren voor team {TeamId} van {From} tot {To}", teamId, from, to);

        var team = await _teamRepository.GetByIdAsync(teamId);
        if (team == null)
            throw new KeyNotFoundException($"Team met id {teamId} niet gevonden.");

        var workOrders = await _workOrderRepository.GetByTeamAndPeriodAsync(teamId, from, to);

        // Verwerk statistieken
        var byStatus = workOrders
            .GroupBy(w => w.Status.ToString())
            .ToDictionary(g => g.Key, g => g.Count());

        var byPriority = workOrders
            .GroupBy(w => w.Priority.ToString())
            .ToDictionary(g => g.Key, g => g.Count());

        // Bouw een eenvoudige tekstuele rapportinhoud
        var content = BuildReportContent(team.Name, from, to, workOrders.Count,
            workOrders.Sum(w => w.EstimatedHours), byStatus, byPriority);

        // Simuleer een kleine vertraging zoals je bij een echte PDF-generator zou hebben
        await Task.Delay(200);

        return new TeamReportDto
        {
            TeamId = teamId,
            TeamName = team.Name,
            From = from,
            To = to,
            TotalWorkOrders = workOrders.Count,
            TotalEstimatedHours = workOrders.Sum(w => w.EstimatedHours),
            WorkOrdersByStatus = byStatus,
            WorkOrdersByPriority = byPriority,
            GeneratedContent = content,
            GeneratedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Genereert rapporten voor meerdere teams PARALLEL met Task.WhenAll.
    /// Dit is de snelle variant: alle rapporten worden tegelijk gestart.
    /// </summary>
    public async Task<IEnumerable<TeamReportDto>> GenerateBulkReportsParallelAsync(
        List<int> teamIds, DateTime from, DateTime to)
    {
        _logger.LogInformation("Parallelle bulk-rapport generatie voor {Count} teams", teamIds.Count);

        // Task.WhenAll start alle taken tegelijkertijd
        var tasks = teamIds.Select(id => GenerateTeamReportAsync(id, from, to));
        var results = await Task.WhenAll(tasks);

        return results;
    }

    /// <summary>
    /// Genereert rapporten voor meerdere teams SEQUENTIEEL (één voor één).
    /// Dit is de langzame variant, ter vergelijking.
    /// </summary>
    public async Task<IEnumerable<TeamReportDto>> GenerateBulkReportsSequentialAsync(
        List<int> teamIds, DateTime from, DateTime to)
    {
        _logger.LogInformation("Sequentiële bulk-rapport generatie voor {Count} teams", teamIds.Count);

        var results = new List<TeamReportDto>();

        // Await elke taak één voor één
        foreach (var id in teamIds)
        {
            var report = await GenerateTeamReportAsync(id, from, to);
            results.Add(report);
        }

        return results;
    }

    /// <summary>
    /// Vergelijkt de snelheid van sequentieel vs. parallel rapportgeneratie.
    /// Didactisch doel: laat het verschil in duur zien.
    /// </summary>
    public async Task<PerformanceComparisonDto> ComparePerformanceAsync(
        List<int> teamIds, DateTime from, DateTime to)
    {
        // Meting 1: Sequentieel
        var swSequential = Stopwatch.StartNew();
        await GenerateBulkReportsSequentialAsync(teamIds, from, to);
        swSequential.Stop();

        // Meting 2: Parallel
        var swParallel = Stopwatch.StartNew();
        await GenerateBulkReportsParallelAsync(teamIds, from, to);
        swParallel.Stop();

        var diff = swSequential.ElapsedMilliseconds - swParallel.ElapsedMilliseconds;

        return new PerformanceComparisonDto
        {
            SequentialDurationMs = swSequential.ElapsedMilliseconds,
            ParallelDurationMs = swParallel.ElapsedMilliseconds,
            DifferenceMs = diff,
            Summary = $"Parallelle verwerking was {diff}ms sneller dan sequentieel voor {teamIds.Count} teams."
        };
    }

    private static string BuildReportContent(
        string teamName, DateTime from, DateTime to,
        int total, double totalHours,
        Dictionary<string, int> byStatus, Dictionary<string, int> byPriority)
    {
        var sb = new StringBuilder();
        sb.AppendLine("=== WERKORDER RAPPORT ===");
        sb.AppendLine($"Team: {teamName}");
        sb.AppendLine($"Periode: {from:dd-MM-yyyy} t/m {to:dd-MM-yyyy}");
        sb.AppendLine($"Gegenereerd op: {DateTime.Now:dd-MM-yyyy HH:mm}");
        sb.AppendLine();
        sb.AppendLine($"Totaal werkorders: {total}");
        sb.AppendLine($"Totale geschatte uren: {totalHours:F1}");
        sb.AppendLine();
        sb.AppendLine("Per status:");
        foreach (var (status, count) in byStatus)
            sb.AppendLine($"  - {status}: {count}");
        sb.AppendLine();
        sb.AppendLine("Per prioriteit:");
        foreach (var (priority, count) in byPriority)
            sb.AppendLine($"  - {priority}: {count}");
        sb.AppendLine("========================");
        return sb.ToString();
    }
}