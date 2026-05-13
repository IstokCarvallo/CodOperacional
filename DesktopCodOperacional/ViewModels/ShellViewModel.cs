using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopCodOperacional.Models.Menu;
using DesktopCodOperacional.Services.Auth;
using DesktopCodOperacional.Services.UI;
using DesktopCodOperacional.ViewModels.Base;
using DesktopCodOperacional.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;

namespace DesktopCodOperacional.ViewModels
{
    public partial class ShellViewModel : BaseViewModel
    {
        private readonly NotificationService _notification;
        private readonly NavigationService _navigation;
        private readonly AuthService _authService;
        private readonly MenuService _menuService;

        public NavigationService Navigation => _navigation;

        public GridLength SidebarWidth =>
                 IsSidebarCollapsed
                     ? new GridLength(88)
                     : new GridLength(280);

        [ObservableProperty]
        private string titulo = "Dashboard";

        [ObservableProperty]
        private bool isSidebarCollapsed;

        [ObservableProperty]
        private string currentRole = string.Empty;

        [ObservableProperty]
        private string currentUser = string.Empty;

        [ObservableProperty]
        private string currentDate = DateTime.Now.ToString("dddd dd MMMM yyyy");

        [ObservableProperty]
        private ObservableCollection<MenuItemModel> menuItems = new();

        public ShellViewModel(NotificationService notification,
                NavigationService navigation,
                AuthService authService,
                MenuService menuService)
        {
            _notification = notification;
            _navigation = navigation;
            _authService = authService;
            _menuService = menuService;

            LoadMenu();

            _navigation.Navigate<DashboardView>();
        }
        private void LoadMenu()
        {
            CurrentRole = _authService.CurrentRole;
            CurrentUser = _authService.CurrentUser;
            MenuItems = _menuService.BuildMenu(CurrentRole);
        }

        [RelayCommand]
        private void ToggleSidebar()
        {
            IsSidebarCollapsed = !IsSidebarCollapsed;
            OnPropertyChanged(nameof(SidebarWidth));
        }

        [RelayCommand]
        private async Task NavigateMenuAsync(MenuItemModel item)
        {
            if (item == null)
                return;

            switch (item.ViewName)
            {
                case "DashboardView":
                    Dashboard();
                    break;

                case "CuartelesView":
                    Cuarteles();
                    break;

                case "PlantasView":
                    Plantas();
                    break;

                case "CambiarPasswordView":
                    CambiarPassword();
                    break;

                case "RegistrarUsuariosView":
                    RegistrarUsuarios();
                    break;

                case "AuditoriaCompletaView":
                    AuditoriaCompleta();
                    break;

                case "AuditoriaPorIdView":
                    AuditoriaPorId();
                    break;

                case "LogoutSesionesView":
                    LogoutSesiones();
                    break;

                case "LogoutTodasView":
                    LogoutTodas();
                    break;

                case "Logout":
                    await LogoutAsync();
                    break;
            }
        }

        [RelayCommand]
        private void Dashboard()
        {
            Titulo = "Dashboard";
            _navigation.Navigate<DashboardView>();
        }

        [RelayCommand]
        private void Cuarteles()
        {
            Titulo = "Cuarteles";
            _navigation.Navigate<CuartelesView>();
        }

        [RelayCommand]
        private void Plantas()
        {
            Titulo = "Plantas";

            // TODO:
        }

        [RelayCommand]
        private void CambiarPassword()
        {
            Titulo = "Cambiar Password";

            // TODO:
        }

        [RelayCommand]
        private void RegistrarUsuarios()
        {
            Titulo = "Registrar Usuarios";

            // TODO:
        }

        [RelayCommand]
        private void AuditoriaCompleta()
        {
            Titulo = "Auditoría Completa";

            // TODO:
        }

        [RelayCommand]
        private void AuditoriaPorId()
        {
            Titulo = "Auditoría por ID";

            // TODO:
        }

        [RelayCommand]
        private void LogoutSesiones()
        {
            Titulo = "Logout Sesiones";

            // TODO:
        }

        [RelayCommand]
        private void LogoutTodas()
        {
            Titulo = "Logout Todas";

            // TODO:
        }

        [RelayCommand]
        private async Task LogoutAsync()
        {
            await _authService.LogoutAsync();

            var loginView = App.AppHost.Services.GetRequiredService<LoginView>();
            
            loginView.Show();

            var windows = Application.Current.Windows
                .OfType<Window>()
                .ToList();

            foreach (var w in windows)
            {
                if (w is ShellWindow)
                    w.Close();
            }
        }
    }
}