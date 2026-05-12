using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopCodOperacional.Services;
using DesktopCodOperacional.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace DesktopCodOperacional.ViewModels
{
    public partial class ShellViewModel : ObservableObject
    {
        private readonly NavigationService _navigation;
        private readonly AuthService _authService;

        public NavigationService Navigation => _navigation;

        [ObservableProperty]
        private string titulo = "Dashboard";

        public ShellViewModel(
            NavigationService navigation,
            AuthService authService)
        {
            _navigation = navigation;
            _authService = authService;

            _navigation.Navigate(new DashboardView());
        }

        [RelayCommand]
        private void Dashboard()
        {
            Titulo = "Dashboard";
            _navigation.Navigate(new DashboardView());
        }

        [RelayCommand]
        private void Cuarteles()
        {
            Titulo = "Cuarteles";

            var view = App.AppHost.Services.GetRequiredService<CuartelesView>();
            _navigation.Navigate(view);
        }

        [RelayCommand]
        private async Task LogoutAsync()
        {
            await _authService.LogoutAsync();

            var loginView = App.AppHost.Services.GetRequiredService<LoginView>();
            loginView.Show();

            foreach (Window w in Application.Current.Windows)
            {
                if (w is ShellWindow)
                    w.Close();
            }
        }
    }
}