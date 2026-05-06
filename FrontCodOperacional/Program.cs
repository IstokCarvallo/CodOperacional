using FrontCodOperacional;
using FrontCodOperacional.Auth;
using FrontCodOperacional.Handlers;
using FrontCodOperacional.Services.Api;
using FrontCodOperacional.Services.Http;
using FrontCodOperacional.Services.UI;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<CustomAuthStateProvider>());
builder.Services.AddScoped<TokenStorage>();
builder.Services.AddScoped<ToastService>();
builder.Services.AddScoped<CurrentUserService>();
builder.Services.AddScoped<AuthMessageHandler>();
builder.Services.AddScoped<HttpErrorHandler>();

builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri("https://localhost:7282/api/");
})
.AddHttpMessageHandler<HttpErrorHandler>()
.AddHttpMessageHandler<AuthMessageHandler>(); 

builder.Services.AddScoped<HttpClient>(sp =>
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("API"));

builder.Services.AddHttpClient("Auth", client =>
{
    client.BaseAddress = new Uri("https://localhost:7282/api/");
});

builder.Services.AddScoped<AuthApiService>(sp =>
{
    var factory = sp.GetRequiredService<IHttpClientFactory>();
    return new AuthApiService(factory.CreateClient("Auth"));
});

builder.Services.AddScoped<PlantasService>();
builder.Services.AddScoped<CuartelesService>();

await builder.Build().RunAsync();