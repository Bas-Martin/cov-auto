using System.Net.Http.Headers;
using System.Net.Http.Json;
using CovAuto.Client.Auth;
using CovAuto.Client.Models;

namespace CovAuto.Client.Services;

public class WorkOrderApiService
{
    private readonly HttpClient _http;
    private readonly JwtAuthStateProvider _authStateProvider;

    public WorkOrderApiService(HttpClient http, JwtAuthStateProvider authStateProvider)
    {
        _http = http;
        _authStateProvider = authStateProvider;
    }

    public async Task<ApiResponse<PagedResult<WorkOrderDto>>?> GetWorkOrdersAsync(WorkOrderQueryParameters query)
    {
        var token = await _authStateProvider.GetTokenAsync();
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var url = $"workorders?{query.ToQueryString()}";
        return await _http.GetFromJsonAsync<ApiResponse<PagedResult<WorkOrderDto>>>(url);
    }

    public async Task<ApiResponse<WorkOrderDto>?> GetWorkOrderAsync(int id)
    {
        var token = await _authStateProvider.GetTokenAsync();
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return await _http.GetFromJsonAsync<ApiResponse<WorkOrderDto>>($"workorders/{id}");
    }

    public async Task<(bool Success, WorkOrderDto? Data, string? Error)> CreateWorkOrderAsync(CreateWorkOrderRequest request)
    {
        var token = await _authStateProvider.GetTokenAsync();
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _http.PostAsJsonAsync("workorders", request);
        if (!response.IsSuccessStatusCode)
        {
            var err = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
            return (false, null, err?.Message ?? "Aanmaken mislukt.");
        }
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<WorkOrderDto>>();
        return (true, result?.Data, null);
    }
}
