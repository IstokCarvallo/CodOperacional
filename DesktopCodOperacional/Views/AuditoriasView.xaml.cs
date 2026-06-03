using DesktopCodOperacional.ViewModels;
using System.Windows.Controls;

namespace DesktopCodOperacional.Views
{
    public partial class AuditoriasView : UserControl
    {
        public AuditoriasView(AuditoriasViewModel vm)
        {
            InitializeComponent();

            DataContext = vm;

            Loaded += async (_, _) =>
            {
                await vm.InicializarAsync();
            };
        }
    }
}
