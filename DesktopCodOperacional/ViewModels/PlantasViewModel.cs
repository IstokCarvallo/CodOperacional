using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopCodOperacional.Models.Planta;
using DesktopCodOperacional.Services.Api;
using DesktopCodOperacional.Services.Export;
using DesktopCodOperacional.Services.UI;
using DesktopCodOperacional.ViewModels.Base;
using DesktopCodOperacional.Views;
using DesktopCodOperacional.Models.Common;
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

            ShowPdf = false;
            ShowPrint = false;
            ShowFilter = true;

            ShowFolder = Visibility.Visible;

            ShowAction = true;
            ActionText = "Editar Código";

            Filters.Add(new FilterField
            {
                Label = "Código",
                PropertyName = "Codigo"
            });

            Filters.Add(new FilterField
            {
                Label = "Código Operacional",
                PropertyName = "CodigoOperacional"
            });

            ConfigureFilters();
        }

        protected override void ConfigureFilters()
        {
            foreach (var filter in Filters)
            {
                filter.ValueChanged += async (_, _) =>
                {
                    await AplicarFiltrosAvanzados();
                };
            }
        }
        protected override async Task ClearFiltersAsync()
        {
            foreach (var filter in Filters)
            {
                filter.Value = string.Empty;
            }

            _allPlantas.Clear();

            PaginaActual = 1;

            await Buscar();
        }

        private async Task AplicarFiltrosAvanzados()
        {
            try
            {
                var filters = GetActiveFilters();

                // SIN FILTROS
                if (!filters.Any())
                {
                    await Buscar();
                    return;
                }

                // CARGAR CACHE COMPLETO
                if (!_allPlantas.Any())
                {
                    var todas = await _service.ObtenerTodasAsync();

                    if (todas != null)
                        _allPlantas = todas;
                }

                var filtered = _allPlantas.AsEnumerable();

                foreach (var filter in filters)
                {
                    filtered = filtered.Where(x =>
                    {
                        var property = x.GetType().GetProperty(filter.Key);

                        if (property == null)
                            return false;

                        var value = property.GetValue(x)?.ToString();

                        if (string.IsNullOrWhiteSpace(value))
                            return false;

                        return value.Contains(filter.Value, StringComparison.OrdinalIgnoreCase);
                    });
                }

                Plantas.Clear();
                ItemsCount = 0;

                foreach (var item in filtered)
                {
                    Plantas.Add(item);
                }

                TotalRegistros = Plantas.Count;
                ItemsCount = Plantas.Count;
                TotalPaginas = 1;
            }
            catch (Exception ex)
            {
                _notification.Warning($"Error filtros: {ex.Message}");
            }
        }

        private Dictionary<string, string> GetActiveFilters()
        {
            return Filters
                .Where(x => !string.IsNullOrWhiteSpace(x.Value))
                .ToDictionary(
                    x => x.PropertyName,
                    x => x.Value);
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

                Plantas.Clear();
                ItemsCount = 0;

                // searchText 
                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    PaginaActual = 1;

                    var data = await _service.BuscarAsync(SearchText);

                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            Plantas.Add(item);
                        }

                        TotalRegistros = Plantas.Count;
                        ItemsCount = Plantas.Count;
                        TotalPaginas = 1;
                    }

                    return;
                }

                // PAGINADO
                var response = await _service.ObtenerPaginadoAsync(PaginaActual, PageSize);

                if (response == null || response.Items == null)
                {
                    _notification.Warning("No se pudo obtener datos o la lista está vacía.");
                    return;
                }

                _allPlantas = response.Items.ToList();

                foreach (var item in response.Items)
                    Plantas.Add(item);
                
                TotalPaginas = response.TotalPaginas;
                ItemsCount = Plantas.Count;
                TotalRegistros = response.TotalRegistros;
            }
            catch (Exception ex)
            {
                _notification.Warning($"Error al cargar datos: {ex.Message}");
            }
            finally
            {
                IsBusy   = false;
            }
        }

        [RelayCommand]
        private async Task SiguientePagina()
        {
            if (!string.IsNullOrWhiteSpace(searchText))
                return;

            if (PaginaActual >= TotalPaginas)
                return;

            PaginaActual++;

            await Buscar();
        }

        [RelayCommand]
        private async Task PaginaAnterior()
        {
            if (!string.IsNullOrWhiteSpace(searchText))
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