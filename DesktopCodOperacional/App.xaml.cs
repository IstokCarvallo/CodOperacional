using DesktopCodOperacional.Models;
using DesktopCodOperacional.Services;
using DesktopCodOperacional.Services.Http;
using DesktopCodOperacional.Services.Security;
using DesktopCodOperacional.ViewModels;
using DesktopCodOperacional.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace DesktopCodOperacional
{
    public partial class App : Application
    {
        public static IHost AppHost { get; private set; }

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    // Config
                    services.Configure<ApiSettings>(context.Configuration.GetSection("ApiSettings"));

                    // Core services
                    services.AddSingleton<TokenStorageService>();
                    services.AddSingleton<AuthService>();
                    services.AddSingleton<NavigationService>();
                    services.AddSingleton<CuartelService>();
                    services.AddSingleton<SecureTokenStorageService>();

                    // HTTP Handler
                    services.AddTransient<TokenHandler>();

                    // Typed HttpClient (ApiService)
                    services.AddHttpClient<ApiService>(client =>
                    {
                        var baseUrl = context.Configuration["ApiSettings:BaseUrl"];
                        client.BaseAddress = new Uri(baseUrl);
                    })
                    .AddHttpMessageHandler<TokenHandler>();

                    // ViewModels
                    services.AddSingleton<LoginViewModel>();
                    services.AddSingleton<ShellViewModel>();
                    services.AddTransient<CuartelesViewModel>();

                    // Views
                    services.AddSingleton<LoginView>();
                    services.AddSingleton<ShellWindow>();
                    services.AddTransient<CuartelesView>();
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost.StartAsync();

            var tokenStorage = AppHost.Services.GetRequiredService<TokenStorageService>();
            var secureStorage = AppHost.Services.GetRequiredService<SecureTokenStorageService>();
            var authService = AppHost.Services.GetRequiredService<AuthService>();

            var saved = secureStorage.Load();

            if (saved != null)
            {
                tokenStorage.SetTokens(saved.Value.accessToken, saved.Value.refreshToken);

                // Opcional pero recomendado: validar sesión
                var refreshed = await authService.RefreshTokenAsync();

                if (refreshed)
                {
                    secureStorage.Save(
                        tokenStorage.GetAccessToken(),
                        tokenStorage.GetRefreshToken());

                    OpenShell();
                    return;
                }

                // Si refresh falla → limpiar
                tokenStorage.Clear();
                secureStorage.Clear();
            }

            OpenLogin();
            base.OnStartup(e);
        }

        private void OpenLogin()
        {
            var loginView = AppHost.Services.GetRequiredService<LoginView>();
            loginView.Show();
        }

        private void OpenShell()
        {
            var shell = AppHost.Services.GetRequiredService<ShellWindow>();
            shell.Show();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost.StopAsync();

            base.OnExit(e);
        }
    }

}
