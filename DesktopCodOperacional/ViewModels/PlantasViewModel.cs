using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopCodOperacional.Models.Planta;
using DesktopCodOperacional.Services.Api;
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
        private readonly NotificationService _notification;

        private const int PageSize = 10;

        [ObservableProperty]
        private bool cargando;

        [ObservableProperty]
        private string searchText  = string.Empty;

        [ObservableProperty]
        private PlantaDto? plantaSeleccionada;

        [ObservableProperty]
        private int paginaActual = 1;

        [ObservableProperty]
        private int totalPaginas;

        [ObservableProperty]
        private int totalRegistros;

        public ObservableCollection<PlantaDto> Plantas { get; set; } = new();

        public PlantasViewModel(PlantaService service, NotificationService notification)
        {
            _service = service;
            _notification = notification;

            // TOOLBAR
            ShowRefresh = true;
            ShowExcel = true;

            ShowPdf = false;
            ShowPrint = false;
            ShowFilter = false;

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
                Cargando = true;

                Plantas.Clear();

                // searchText 
                if (!string.IsNullOrWhiteSpace(searchText ))
                {
                    PaginaActual = 1;

                    var data = await _service.BuscarAsync(searchText );

                    if (data != null)
                    {
                        foreach (var item in data)
                            Plantas.Add(item);
                        
                        TotalRegistros = Plantas.Count;
                        TotalPaginas = 1;
                    }
                    else
                        _notification.Warning("No se encontraron resultados para el searchText .");
                    
                    return;
                }

                // PAGINADO
                var response = await _service.ObtenerPaginadoAsync(PaginaActual, PageSize);

                if (response == null || response.Items == null)
                {
                    _notification.Warning("No se pudo obtener datos o la lista está vacía.");

                    return;
                }

                foreach (var item in response.Items)
                    Plantas.Add(item);
                
                TotalPaginas = response.TotalPaginas;
                TotalRegistros = response.TotalRegistros;
            }
            catch (Exception ex)
            {
                _notification.Warning($"Error al cargar datos: {ex.Message}");
            }
            finally
            {
                Cargando = false;
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

        protected override async Task ExcelAsync()
        {
            _notification.Info("Exportación Excel próximamente");

            await Task.CompletedTask;
        }

        protected override async Task ActionAsync()
        {
            await EditarCodigo();
        }
    }
}