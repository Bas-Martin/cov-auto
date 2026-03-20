using System.Net.Http.Json;
using CovAuto.Client.Models;

namespace CovAuto.Client.Services;

public class WorkOrderApiService
{
    private readonly HttpClient _http;

    public WorkOrderApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<ApiResponse<PagedResult<WorkOrderDto>>?> GetWorkOrdersAsync(WorkOrderQueryParameters query)
    {
        var url = $"workorders?{query.ToQueryString()}";
        return await _http.GetFromJsonAsync<ApiResponse<PagedResult<WorkOrderDto>>>(url);
    }

    public async Task<ApiResponse<WorkOrderDto>?> GetWorkOrderAsync(int id)
        => await _http.GetFromJsonAsync<ApiResponse<WorkOrderDto>>($"workorders/{id}");

    public async Task<(bool Success, WorkOrderDto? Data, string? Error)> CreateWorkOrderAsync(CreateWorkOrderRequest request)
    {
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
