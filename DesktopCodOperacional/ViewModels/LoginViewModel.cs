using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopCodOperacional.Services.Auth;
using DesktopCodOperacional.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace DesktopCodOperacional.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly AuthService _authService;


        [ObservableProperty]
        private string usuario = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private bool cargando;

        public LoginViewModel(AuthService authService)
        {
            _authService = authService;
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
                    MessageBox.Show("Credenciales inválidas");
                }
            }
            finally
            {
                Cargando = false;
            }
        }
    }
}
