using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopCodOperacional.Models.Cuartel;
using DesktopCodOperacional.Services.Api;
using DesktopCodOperacional.Views;
using System.Collections.ObjectModel;
using System.Windows;

namespace DesktopCodOperacional.ViewModels
{
    public partial class CuartelesViewModel : ObservableObject
    {
        private readonly CuartelService _service;

        [ObservableProperty]
        private bool cargando;

        // FILTROS
        [ObservableProperty]
        private string filtroProductor = string.Empty;

        [ObservableProperty]
        private ProductorDto? productorSeleccionado;

        [ObservableProperty]
        private string filtroPredio = string.Empty;

        [ObservableProperty]
        private PredioDto? predioSeleccionado;

        [ObservableProperty]
        private CuartelDto? cuartelSeleccionado;

        // COLECCIONES
        public ObservableCollection<ProductorDto>
            Productores { get; set; } = new();

        public ObservableCollection<ProductorDto>
            ProductoresFiltrados { get; set; } = new();

        public ObservableCollection<PredioDto>
            Predios { get; set; } = new();

        public ObservableCollection<PredioDto>
            PrediosFiltrados { get; set; } = new();

        public ObservableCollection<CuartelDto>
            Cuarteles { get; set; } = new();

        public CuartelesViewModel(CuartelService service)
        {
            _service = service;
        }

        [RelayCommand]
        private async Task Inicializar()
        {
            try
            {
                Cargando = true;

                await CargarProductores();
            }
            finally
            {
                Cargando = false;
            }
        }

        [RelayCommand]
        private async Task EditarCodigo()
        {
            if (CuartelSeleccionado == null)
                return;

            var window = new EditarCodigoOperacionalWindow();

            var vm =
                new EditarCodigoOperacionalViewModel(CuartelSeleccionado, _service, window);

            window.DataContext = vm;

            window.Owner = Application.Current.MainWindow;

            var result =
                window.ShowDialog();

            if (result == true)
            {
                await CargarCuartelesAsync();
            }
        }

        private async Task CargarProductores()
        {
            Productores.Clear();
            ProductoresFiltrados.Clear();

            var data =
                await _service.ObtenerProductoresAsync();

            foreach (var item in data)
            {
                Productores.Add(item);
                ProductoresFiltrados.Add(item);
            }
        }

        private async Task CargarPrediosAsync()
        {
            Predios.Clear();
            PrediosFiltrados.Clear();
            PredioSeleccionado = null;

            if (ProductorSeleccionado == null)
                return;

            var data = await _service.ObtenerPrediosAsync(ProductorSeleccionado.Codigo);

            foreach (var item in data)
            {
                Predios.Add(item);
                PrediosFiltrados.Add(item);
            }
        }

        private async Task CargarCuartelesAsync()
        {
            Cuarteles.Clear();
            CuartelSeleccionado = null;

            if (ProductorSeleccionado == null)
                return;

            if (PredioSeleccionado == null)
                return;

            try
            {
                Cargando = true;

                var data = await _service.ObtenerCuartelesAsync(
                        ProductorSeleccionado.Codigo,
                        PredioSeleccionado.Codigo);

                foreach (var item in data)
                {
                    Cuarteles.Add(item);
                }
            }
            finally
            {
                Cargando = false;
            }
        }

        partial void OnProductorSeleccionadoChanged(ProductorDto? value)
        {
            _ = CargarPrediosAsync();
        }
        partial void OnPredioSeleccionadoChanged(PredioDto? value)
        {
            _ = CargarCuartelesAsync();
        }

        partial void OnFiltroProductorChanged(string value)
        {
            FiltrarProductores(value);
        }
        partial void OnFiltroPredioChanged(string value)
        {
            FiltrarPredios(value);
        }

        private void FiltrarProductores(string texto)
        {
            ProductoresFiltrados.Clear();

            var filtro = texto?.Trim().ToLower() ?? "";

            var items =
                Productores.Where(x =>
                    x.Codigo.ToString().Contains(filtro)
                    || x.Nombre.ToLower().Contains(filtro));

            foreach (var item in items)
            {
                ProductoresFiltrados.Add(item);
            }
        }

        private void FiltrarPredios(string texto)
        {
            PrediosFiltrados.Clear();

            var filtro = texto?.Trim().ToLower() ?? "";

            var items =
                Predios.Where(x =>
                    x.Codigo.ToString().Contains(filtro)
                    || x.Nombre.ToLower().Contains(filtro));

            foreach (var item in items)
            {
                PrediosFiltrados.Add(item);
            }
        }
    }
}
