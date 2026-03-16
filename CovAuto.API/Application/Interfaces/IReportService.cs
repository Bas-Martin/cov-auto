using CovAuto.API.Application.DTOs;

namespace CovAuto.API.Application.Interfaces;

public interface IReportService
{
    Task<TeamReportDto> GenerateTeamReportAsync(int teamId, DateTime from, DateTime to);
    Task<IEnumerable<TeamReportDto>> GenerateBulkReportsParallelAsync(List<int> teamIds, DateTime from, DateTime to);
    Task<IEnumerable<TeamReportDto>> GenerateBulkReportsSequentialAsync(List<int> teamIds, DateTime from, DateTime to);
    Task<PerformanceComparisonDto> ComparePerformanceAsync(List<int> teamIds, DateTime from, DateTime to);
}
