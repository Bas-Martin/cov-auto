using System.Net.Http.Headers;
using System.Net.Http.Json;
using CovAuto.Client.Auth;
using CovAuto.Client.Models;

namespace CovAuto.Client.Services;

public class TeamApiService
{
    private readonly HttpClient _http;
    private readonly JwtAuthStateProvider _authStateProvider;

    public TeamApiService(HttpClient http, JwtAuthStateProvider authStateProvider)
    {
        _http = http;
        _authStateProvider = authStateProvider;
    }

    public async Task<ApiResponse<IEnumerable<ServiceTeamDto>>?> GetTeamsAsync()
    {
        var token = await _authStateProvider.GetTokenAsync();
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return await _http.GetFromJsonAsync<ApiResponse<IEnumerable<ServiceTeamDto>>>("teams");
    }

    public async Task<ApiResponse<ServiceTeamDto>?> GetTeamAsync(int id)
    {
        var token = await _authStateProvider.GetTokenAsync();
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return await _http.GetFromJsonAsync<ApiResponse<ServiceTeamDto>>($"teams/{id}");
    }
}
