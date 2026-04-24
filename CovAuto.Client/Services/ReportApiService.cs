using System.Net.Http.Headers;
using System.Net.Http.Json;
using CovAuto.Client.Auth;
using CovAuto.Client.Models;

namespace CovAuto.Client.Services;

public class ReportApiService
{
    private readonly HttpClient _http;
    private readonly JwtAuthStateProvider _authStateProvider;

    public ReportApiService(HttpClient http, JwtAuthStateProvider authStateProvider)
    {
        _http = http;
        _authStateProvider = authStateProvider;
    }

    public async Task<ApiResponse<TeamReportDto>?> GenerateTeamReportAsync(int teamId, TeamReportRequest request)
    {
        var token = await _authStateProvider.GetTokenAsync();
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _http.PostAsJsonAsync($"reports/workorders/team/{teamId}", request);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<ApiResponse<TeamReportDto>>();
    }

    public async Task<ApiResponse<IEnumerable<TeamReportDto>>?> GenerateBulkReportsAsync(BulkReportRequest request)
    {
        var token = await _authStateProvider.GetTokenAsync();
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _http.PostAsJsonAsync("reports/workorders/bulk", request);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<TeamReportDto>>>();
    }

    public async Task<ApiResponse<PerformanceComparisonDto>?> GetPerformanceComparisonAsync()
    {
        var token = await _authStateProvider.GetTokenAsync();
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return await _http.GetFromJsonAsync<ApiResponse<PerformanceComparisonDto>>("reports/performance-comparison");
    }
}
