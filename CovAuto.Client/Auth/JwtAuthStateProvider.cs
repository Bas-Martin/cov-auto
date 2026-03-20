using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;

namespace CovAuto.Client.Auth;

public class JwtAuthStateProvider : AuthenticationStateProvider
{
    private const string TokenKey = "auth_token";
    private readonly SessionStorageService _sessionStorage;

    public JwtAuthStateProvider(SessionStorageService sessionStorage)
    {
        _sessionStorage = sessionStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _sessionStorage.GetItemAsync(TokenKey);

        if (string.IsNullOrWhiteSpace(token))
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        var claims = ParseClaimsFromJwt(token);
        var identity = new ClaimsIdentity(claims, "jwt");
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public async Task MarkUserAsAuthenticated(string token)
    {
        await _sessionStorage.SetItemAsync(TokenKey, token);
        var claims = ParseClaimsFromJwt(token);
        var identity = new ClaimsIdentity(claims, "jwt");
        var user = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public async Task MarkUserAsLoggedOut()
    {
        await _sessionStorage.RemoveItemAsync(TokenKey);
        var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
    }

    public async Task<string?> GetTokenAsync()
        => await _sessionStorage.GetItemAsync(TokenKey);

    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();
        var payload = jwt.Split('.')[1];

        var padded = payload.Length % 4 == 0 ? payload
            : payload + new string('=', 4 - payload.Length % 4);
        var base64 = padded.Replace('-', '+').Replace('_', '/');

        byte[] jsonBytes;
        try { jsonBytes = Convert.FromBase64String(base64); }
        catch { return claims; }

        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonBytes);
        if (keyValuePairs == null) return claims;

        foreach (var kvp in keyValuePairs)
        {
            var claimType = kvp.Key switch
            {
                "sub" => ClaimTypes.NameIdentifier,
                "email" => ClaimTypes.Email,
                "unique_name" or "name" => ClaimTypes.Name,
                "role" or "http://schemas.microsoft.com/ws/2008/06/identity/claims/role" => ClaimTypes.Role,
                _ => kvp.Key
            };

            if (kvp.Value.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in kvp.Value.EnumerateArray())
                    claims.Add(new Claim(claimType, item.GetString() ?? string.Empty));
            }
            else
            {
                claims.Add(new Claim(claimType, kvp.Value.GetString() ?? kvp.Value.ToString()));
            }
        }

        return claims;
    }
}
