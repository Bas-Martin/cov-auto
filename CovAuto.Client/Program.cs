using CovAuto.Client.Auth;
using CovAuto.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<CovAuto.Client.App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5239/";
if (!apiBaseUrl.EndsWith('/')) apiBaseUrl += '/';

builder.Services.AddSingleton<SessionStorageService>();
builder.Services.AddSingleton<JwtAuthStateProvider>();
builder.Services.AddSingleton<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<JwtAuthStateProvider>());

builder.Services.AddTransient<AuthTokenHandler>();
builder.Services.AddHttpClient("CovAutoAPI", client =>
    client.BaseAddress = new Uri(apiBaseUrl))
    .AddHttpMessageHandler<AuthTokenHandler>();

builder.Services.AddHttpClient("CovAutoPublic", client =>
    client.BaseAddress = new Uri(apiBaseUrl));

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthService>(sp =>
{
    var factory = sp.GetRequiredService<IHttpClientFactory>();
    var http = factory.CreateClient("CovAutoPublic");
    var authProvider = sp.GetRequiredService<JwtAuthStateProvider>();
    return new AuthService(http, authProvider);
});

builder.Services.AddScoped<WorkOrderApiService>(sp =>
{
    var factory = sp.GetRequiredService<IHttpClientFactory>();
    return new WorkOrderApiService(factory.CreateClient("CovAutoAPI"));
});
builder.Services.AddScoped<TeamApiService>(sp =>
{
    var factory = sp.GetRequiredService<IHttpClientFactory>();
    return new TeamApiService(factory.CreateClient("CovAutoAPI"));
});
builder.Services.AddScoped<ReportApiService>(sp =>
{
    var factory = sp.GetRequiredService<IHttpClientFactory>();
    return new ReportApiService(factory.CreateClient("CovAutoAPI"));
});

await builder.Build().RunAsync();
