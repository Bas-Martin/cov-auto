using System.Net.Http.Json;
using CovAuto.Client.Models;

namespace CovAuto.Client.Services;

public class ReportApiService
{
    private readonly HttpClient _http;

    public ReportApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<ApiResponse<TeamReportDto>?> GenerateTeamReportAsync(int teamId, TeamReportRequest request)
    {
        var response = await _http.PostAsJsonAsync($"reports/workorders/team/{teamId}", request);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<ApiResponse<TeamReportDto>>();
    }

    public async Task<ApiResponse<IEnumerable<TeamReportDto>>?> GenerateBulkReportsAsync(BulkReportRequest request)
    {
        var response = await _http.PostAsJsonAsync("reports/workorders/bulk", request);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<TeamReportDto>>>();
    }

    public async Task<ApiResponse<PerformanceComparisonDto>?> GetPerformanceComparisonAsync()
        => await _http.GetFromJsonAsync<ApiResponse<PerformanceComparisonDto>>("reports/performance-comparison");
}
