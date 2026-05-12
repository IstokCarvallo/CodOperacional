using DesktopCodOperacional.ViewModels;
using System.Windows;

namespace DesktopCodOperacional.Views
{
    public partial class LoginView : Window
    {
        public LoginView(LoginViewModel vm)
        {
            InitializeComponent();

            DataContext = vm;

            PasswordInput.PasswordChanged += (s, e) =>
            {
                vm.Password = PasswordInput.Password;
            };
        }        
    }
}
