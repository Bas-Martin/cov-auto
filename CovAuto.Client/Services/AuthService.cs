using System.Net.Http.Json;
using CovAuto.Client.Auth;
using CovAuto.Client.Models;

namespace CovAuto.Client.Services;

public class AuthService
{
    private readonly HttpClient _http;
    private readonly JwtAuthStateProvider _authStateProvider;

    public AuthService(HttpClient http, JwtAuthStateProvider authStateProvider)
    {
        _http = http;
        _authStateProvider = authStateProvider;
    }

    public async Task<(bool Success, string? ErrorMessage)> LoginAsync(LoginRequest request)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("auth/login", request);
            if (!response.IsSuccessStatusCode)
                return (false, "Ongeldige gebruikersnaam of wachtwoord.");

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResponse>>();
            if (result?.Success == true && result.Data?.Token != null)
            {
                await _authStateProvider.MarkUserAsAuthenticated(result.Data.Token);
                return (true, null);
            }

            return (false, result?.Message ?? "Inloggen mislukt.");
        }
        catch
        {
            return (false, "Kan geen verbinding maken met de API. Zorg dat de API actief is op http://localhost:5239.");
        }
    }

    public async Task LogoutAsync()
        => await _authStateProvider.MarkUserAsLoggedOut();
}
