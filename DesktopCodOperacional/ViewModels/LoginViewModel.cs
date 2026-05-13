using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopCodOperacional.Services.Auth;
using DesktopCodOperacional.Services.UI;
using DesktopCodOperacional.ViewModels.Base;
using DesktopCodOperacional.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace DesktopCodOperacional.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly AuthService _authService;
        private readonly NotificationService _notification;

        [ObservableProperty]
        private string usuario = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private bool cargando;

        public LoginViewModel(NotificationService notification, AuthService authService)
        {
            _authService = authService;
            _notification = notification;
        }

        [RelayCommand]
        private async Task IniciarSesion()
        {
            try
            {
                Cargando = true;

                var ok = await _authService.LoginAsync(Usuario, Password);

                if (ok)
                {
                    var shell = App.AppHost.Services.GetRequiredService<ShellWindow>();

                    Application.Current.MainWindow = shell;

                    shell.Show();

                    Application.Current.Windows
                        .OfType<Window>()
                        .SingleOrDefault(w => w is LoginView)?
                        .Close();
                }
                else
                {
                    _notification.Error("Usuario o contraseña incorrectos");
                }
            }
            finally
            {
                Cargando = false;
            }
        }
    }
}
