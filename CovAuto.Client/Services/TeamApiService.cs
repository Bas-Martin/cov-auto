using System.Net.Http.Json;
using CovAuto.Client.Models;

namespace CovAuto.Client.Services;

public class TeamApiService
{
    private readonly HttpClient _http;

    public TeamApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<ApiResponse<IEnumerable<ServiceTeamDto>>?> GetTeamsAsync()
        => await _http.GetFromJsonAsync<ApiResponse<IEnumerable<ServiceTeamDto>>>("teams");

    public async Task<ApiResponse<ServiceTeamDto>?> GetTeamAsync(int id)
        => await _http.GetFromJsonAsync<ApiResponse<ServiceTeamDto>>($"teams/{id}");
}
