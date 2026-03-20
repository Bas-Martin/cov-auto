using Microsoft.JSInterop;

namespace CovAuto.Client.Auth;

public class SessionStorageService
{
    private readonly IJSRuntime _js;

    public SessionStorageService(IJSRuntime js)
    {
        _js = js;
    }

    public async Task SetItemAsync(string key, string value)
        => await _js.InvokeVoidAsync("sessionStorage.setItem", key, value);

    public async Task<string?> GetItemAsync(string key)
        => await _js.InvokeAsync<string?>("sessionStorage.getItem", key);

    public async Task RemoveItemAsync(string key)
        => await _js.InvokeVoidAsync("sessionStorage.removeItem", key);
}
