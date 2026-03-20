using System.Net.Http.Headers;

namespace CovAuto.Client.Auth;

public class AuthTokenHandler : DelegatingHandler
{
    private readonly JwtAuthStateProvider _authStateProvider;

    public AuthTokenHandler(JwtAuthStateProvider authStateProvider)
    {
        _authStateProvider = authStateProvider;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _authStateProvider.GetTokenAsync();
        if (!string.IsNullOrEmpty(token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return await base.SendAsync(request, cancellationToken);
    }
}
