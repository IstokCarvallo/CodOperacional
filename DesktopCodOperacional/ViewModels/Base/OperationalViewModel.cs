using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace DesktopCodOperacional.ViewModels.Base
{
    public partial class OperationalViewModel : BaseViewModel
    {
        // =========================================
        // SEARCH
        // =========================================

        [ObservableProperty]
        private string searchText = string.Empty;

        // =========================================
        // TOOLBAR FLAGS
        // =========================================

        [ObservableProperty]
        private bool showRefresh = true;

        [ObservableProperty]
        private bool showFilter;

        [ObservableProperty]
        private bool showExcel = true;

        [ObservableProperty]
        private bool showPdf;

        [ObservableProperty]
        private bool showPrint;

        [ObservableProperty]
        private bool showAction;

        [ObservableProperty]
        private string actionText = "Acción";

        // =========================================
        // COMMANDS
        // =========================================

        [RelayCommand]
        protected virtual async Task RefreshAsync()
        {
            await Task.CompletedTask;
        }

        [RelayCommand]
        protected virtual async Task FilterAsync()
        {
            await Task.CompletedTask;
        }

        [RelayCommand]
        protected virtual async Task ExcelAsync()
        {
            await Task.CompletedTask;
        }

        [RelayCommand]
        protected virtual async Task PdfAsync()
        {
            await Task.CompletedTask;
        }

        [RelayCommand]
        protected virtual async Task PrintAsync()
        {
            await Task.CompletedTask;
        }

        [RelayCommand]
        protected virtual async Task ActionAsync()
        {
            await Task.CompletedTask;
        }
    }
}
