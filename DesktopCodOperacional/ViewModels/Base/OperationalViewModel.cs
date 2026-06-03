using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopCodOperacional.Models.Common;
using System.Collections.ObjectModel;
using System.Windows;

namespace DesktopCodOperacional.ViewModels.Base
{
    public partial class OperationalViewModel : BaseViewModel
    {
        public bool HasData => ItemsCount > 0;

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private int itemsCount;

        [ObservableProperty]
        private bool filtersVisible;
        // SEARCH
        [ObservableProperty]
        private string searchText = string.Empty;
        public ObservableCollection<FilterField> Filters { get; set; } = new();

        // TOOLBAR FLAGS
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
        private bool showFolder = false;

        [ObservableProperty]
        private bool showAction;

        [ObservableProperty]
        private string actionText = "Acción";

        protected virtual void ConfigureFilters()
        {}
        partial void OnItemsCountChanged(int value)
        {
            OnPropertyChanged(nameof(HasData));
        }

        [RelayCommand]
        protected virtual async Task ClearFiltersAsync()
        {
            await Task.CompletedTask;
        }

        // COMMANDS
        [RelayCommand]
        protected virtual async Task RefreshAsync()
        {
            await Task.CompletedTask;
        }

        [RelayCommand]
        protected virtual async Task FilterAsync()
        {
            FiltersVisible = !FiltersVisible;
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
        protected virtual async Task OpenFolderAsync()
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
