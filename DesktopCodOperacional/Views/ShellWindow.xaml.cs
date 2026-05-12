using DesktopCodOperacional.ViewModels;
using System.Windows;

namespace DesktopCodOperacional.Views
{
    public partial class ShellWindow : Window
    {
        public ShellWindow(ShellViewModel vm)
        {
            InitializeComponent();

            DataContext = vm;
        }
    }
}
