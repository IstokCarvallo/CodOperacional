using DesktopCodOperacional.ViewModels;
using System.Windows.Controls;

namespace DesktopCodOperacional.Views
{
    public partial class CuartelesView : UserControl
    {
        public CuartelesView(CuartelesViewModel vm)
        {
            InitializeComponent();

            DataContext = vm;

            Loaded += async (s, e) =>
            {
                await vm.InicializarCommand.ExecuteAsync(null);
            };
        }
    }
}
