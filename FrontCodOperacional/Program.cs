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

// ✅ HttpClient 
builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri("https://localhost:7282/api/");
})
.AddHttpMessageHandler<AuthMessageHandler>();

// CLIENTE SIN JWT (AUTH)
builder.Services.AddHttpClient("Auth", client =>
{
    client.BaseAddress = new Uri("https://localhost:7282/api/");
});

// HttpClient principal
builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("API"));

// AuthApiService (CORRECTO)
builder.Services.AddScoped<AuthApiService>(sp =>
{
    var factory = sp.GetRequiredService<IHttpClientFactory>();
    return new AuthApiService(factory.CreateClient("Auth"));
});

await builder.Build().RunAsync();