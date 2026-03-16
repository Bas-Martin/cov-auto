using CovAuto.API.Application.DTOs;
using CovAuto.API.Application.Interfaces;
using CovAuto.API.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CovAuto.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "Planner")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    /// <summary>
    /// Genereert een rapport voor één team over een opgegeven periode.
    /// </summary>
    [HttpPost("workorders/team/{teamId:int}")]
    [ProducesResponseType(typeof(ApiResponse<TeamReportDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GenerateTeamReport(int teamId, [FromBody] TeamReportRequest request)
    {
        try
        {
            var report = await _reportService.GenerateTeamReportAsync(teamId, request.From, request.To);
            return Ok(ApiResponse<TeamReportDto>.Ok(report));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<string>.Fail(ex.Message));
        }
    }

    /// <summary>
    /// Genereert rapporten voor meerdere teams PARALLEL (snel).
    /// Gebruikt Task.WhenAll voor gelijktijdige verwerking.
    /// </summary>
    [HttpPost("workorders/bulk")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<TeamReportDto>>), 200)]
    public async Task<IActionResult> GenerateBulkReports([FromBody] BulkReportRequest request)
    {
        var reports = await _reportService.GenerateBulkReportsParallelAsync(
            request.TeamIds, request.From, request.To);

        return Ok(ApiResponse<IEnumerable<TeamReportDto>>.Ok(reports,
            $"Rapporten parallel gegenereerd voor {request.TeamIds.Count} teams."));
    }

    /// <summary>
    /// Vergelijkt de verwerkingstijd van sequentieel vs. parallel rapportgeneratie.
    /// Didactisch doel: toon het verschil in duur.
    /// </summary>
    [HttpGet("performance-comparison")]
    [ProducesResponseType(typeof(ApiResponse<PerformanceComparisonDto>), 200)]
    public async Task<IActionResult> PerformanceComparison()
    {
        // Gebruik beide teams voor de vergelijking
        var teamIds = new List<int> { 1, 2 };
        var from = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var to = new DateTime(2024, 12, 31, 0, 0, 0, DateTimeKind.Utc);

        var comparison = await _reportService.ComparePerformanceAsync(teamIds, from, to);
        return Ok(ApiResponse<PerformanceComparisonDto>.Ok(comparison));
    }
}
