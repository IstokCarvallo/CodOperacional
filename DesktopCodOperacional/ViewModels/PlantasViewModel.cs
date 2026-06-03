using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopCodOperacional.Models.Planta;
using DesktopCodOperacional.Services.Api;
using DesktopCodOperacional.Services.Export;
using DesktopCodOperacional.Services.UI;
using DesktopCodOperacional.ViewModels.Base;
using DesktopCodOperacional.Views;
using System.Collections.ObjectModel;
using System.Windows;

namespace DesktopCodOperacional.ViewModels
{
    public partial class PlantasViewModel : OperationalViewModel
    {
        private readonly PlantaService _service;
        private readonly ExportService _exportService;
        private readonly NotificationService _notification;

        private const int PageSize = 15;
        private List<PlantaDto> _allPlantas = [];

        [ObservableProperty]
        private string searchText = string.Empty;

        [ObservableProperty]
        private PlantaDto? plantaSeleccionada;

        [ObservableProperty]
        private int paginaActual = 1;

        [ObservableProperty]
        private int totalPaginas;

        [ObservableProperty]
        private int totalRegistros;
        private bool IsSearching => !string.IsNullOrWhiteSpace(SearchText);
        public ObservableCollection<PlantaDto> Plantas { get; set; } = new();
        public PlantasViewModel(PlantaService service, 
                NotificationService notification,
                ExportService exportService)
        {
            _service = service;
            _notification = notification;
            _exportService = exportService;

            // TOOLBAR
            ShowRefresh = true;
            ShowExcel = true;
            ShowPdf = true;
            ShowPrint = true;
            ShowFilter = false;
            ShowFolder = true;

            ShowAction = true;
            ActionText = "Editar Código";       
        }
        partial void OnSearchTextChanged(string value)
        {
            _ = Buscar();
        }

        [RelayCommand]
        private async Task Inicializar()
        {
            await Buscar();
        }

        [RelayCommand]
        private async Task EditarCodigo()
        {
            if (PlantaSeleccionada == null)
            {
                _notification.Warning("Debe seleccionar una planta");
                return;
            }

            var window = new EditarCodigoOperacionalPlantaView();

            var vm = new EditarCodigoOperacionalPlantaViewModel(_notification, PlantaSeleccionada, _service, window);

            window.DataContext = vm;

            window.Owner = Application.Current.MainWindow;

            var result = window.ShowDialog();

            if (result == true)
            {
                await Buscar();
            }
        }

        [RelayCommand]
        private async Task Buscar()
        {
            try
            {
                IsBusy = true;

                var data = string.IsNullOrWhiteSpace(SearchText)
                    ? await _service.ObtenerPaginadoAsync(PaginaActual, PageSize)
                    : null;

                Plantas.Clear();

                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    PaginaActual = 1;

                    var result = await _service.BuscarAsync(SearchText);

                    if (result != null)
                    {
                        foreach (var item in result)
                            Plantas.Add(item);

                        TotalRegistros = Plantas.Count;
                        TotalPaginas = 1;
                    }

                    return;
                }

                if (data == null || data.Items == null)
                {
                    _notification.Warning("Sin datos");
                    return;
                }

                _allPlantas = data.Items.ToList();

                foreach (var item in data.Items)
                    Plantas.Add(item);

                TotalPaginas = data.TotalPaginas;
                TotalRegistros = data.TotalRegistros;
                ItemsCount = Plantas.Count;
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task SiguientePagina()
        {
            if (IsSearching)
                return;

            if (PaginaActual >= TotalPaginas)
                return;

            PaginaActual++;

            await Buscar();
        }

        [RelayCommand]
        private async Task PaginaAnterior()
        {
            if (IsSearching)
                return;

            if (PaginaActual <= 1)
                return;

            PaginaActual--;

            await Buscar();
        }

        public async Task LoadAsync()
        {
            await Buscar();
        }

        // TOOLBAR
        protected override async Task RefreshAsync()
        {
            await Buscar();
            _notification.Success("Datos actualizados");
        }
        protected override async Task PrintAsync()
        {
            try
            { 
                IsBusy = true;
                await _exportService.PrintAsync(Plantas, "Listado de Plantas");
                _notification.Success("Documento enviado a impresión");
            }
            finally
            {
                IsBusy = false;
            }

        }

        protected override async Task PdfAsync()
        {
            try
            {
                IsBusy = true;
                await _exportService.ExportToPdfAsync(Plantas, "Listado de Plantas");
                _notification.Success("PDF generado correctamente");
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected override async Task ExcelAsync()
        {
            try
            {
                IsBusy = true;
                await _exportService.ExportToExcelAsync(Plantas, "Plantas");
                _notification.Success("Excel generado correctamente");
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected override Task OpenFolderAsync()
        {
            _exportService.OpenExportFolder();
            return Task.CompletedTask;
        }
        protected override async Task ActionAsync()
        {
            await EditarCodigo();
        }
    }
}