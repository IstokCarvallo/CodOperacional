using FrontCodOperacional;
using FrontCodOperacional.Auth;
using FrontCodOperacional.Handlers;
using FrontCodOperacional.Services.Api;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");

// Auth
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<CustomAuthStateProvider>());

// Storage
builder.Services.AddScoped<TokenStorage>();

// Handler JWT
builder.Services.AddScoped<AuthMessageHandler>();

// ✅ HttpClient CORRECTO (sin factory)
builder.Services.AddScoped(sp =>
{
    var handler = sp.GetRequiredService<AuthMessageHandler>();

    // 🔴 CLAVE (esto evita tu error anterior)
    handler.InnerHandler = new HttpClientHandler();

    return new HttpClient(handler)
    {
        BaseAddress = new Uri("https://localhost:7282/api/")
    };
});

// Servicios API
builder.Services.AddScoped<AuthApiService>();

await builder.Build().RunAsync();