using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace DesktopCodOperacional.Services.UI
{
    public partial class NavigationService : ObservableObject
    {
        private readonly IServiceProvider _provider;

        [ObservableProperty]
        private UserControl? currentView;

        public NavigationService(IServiceProvider provider)
        {
            _provider = provider;
        }

        public void Navigate<T>()
            where T : UserControl
        {
            CurrentView = _provider.GetRequiredService<T>();
        }
    }
}
