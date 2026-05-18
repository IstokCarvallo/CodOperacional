using DesktopCodOperacional.ViewModels;
using System.Windows.Controls;

namespace DesktopCodOperacional.Views
{
    public partial class PlantasView : UserControl
    {
        public PlantasView(PlantasViewModel vm)
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