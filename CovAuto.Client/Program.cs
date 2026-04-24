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

builder.Services.AddSingleton<JwtAuthStateProvider>();
builder.Services.AddSingleton<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<JwtAuthStateProvider>());

builder.Services.AddTransient<AuthTokenHandler>();

// Login gebruikt geen token, dus geen AuthTokenHandler
builder.Services.AddHttpClient<AuthService>(client =>
    client.BaseAddress = new Uri(apiBaseUrl));

// Overige API-aanroepen krijgen automatisch het Bearer token via AuthTokenHandler
builder.Services.AddHttpClient<WorkOrderApiService>(client =>
    client.BaseAddress = new Uri(apiBaseUrl))
    .AddHttpMessageHandler<AuthTokenHandler>();

builder.Services.AddHttpClient<TeamApiService>(client =>
    client.BaseAddress = new Uri(apiBaseUrl))
    .AddHttpMessageHandler<AuthTokenHandler>();

builder.Services.AddHttpClient<ReportApiService>(client =>
    client.BaseAddress = new Uri(apiBaseUrl))
    .AddHttpMessageHandler<AuthTokenHandler>();

builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
