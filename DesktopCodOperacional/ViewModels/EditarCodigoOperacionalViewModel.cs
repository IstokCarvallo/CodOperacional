using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopCodOperacional.Models.Cuartel;
using DesktopCodOperacional.Services.Api;
using System.Windows;

namespace DesktopCodOperacional.ViewModels
{
    public partial class EditarCodigoOperacionalViewModel : ObservableObject
    {
        private readonly CuartelService _service;
        private readonly Window _window;

        public int Productor { get; }
        public int Predio { get; }
        public int CodigoCuartel { get; }
        public string NombreCuartel { get; }

        [ObservableProperty]
        private string codigoOperacional = string.Empty;

        [ObservableProperty]
        private bool guardando;

        public EditarCodigoOperacionalViewModel(
                    CuartelDto cuartel,
                    CuartelService service,
                    Window window)
        {
            _service = service;
            _window = window;
            Productor = cuartel.Productor;
            Predio = cuartel.Predio;
            CodigoCuartel = cuartel.CodigoCuartel;
            NombreCuartel = cuartel.Nombre;

            CodigoOperacional = cuartel.CodigoOperacional ?? "";
        }

        partial void OnCodigoOperacionalChanged(string value)
        {
            CodigoOperacional = value?.ToUpper() ?? "";
        }

        [RelayCommand]
        private async Task Guardar()
        {
            if (string.IsNullOrWhiteSpace(CodigoOperacional))
            {
                MessageBox.Show("Debe ingresar el código operacional");
                return;
            }
            try
            {
                Guardando = true;

                var dto = new UpdateCodigoOperacionalDto
                {
                    Productor = Productor,
                    Predio = Predio,
                    CodigoCuartel = CodigoCuartel,
                    CodigoOperacional = CodigoOperacional.Trim()
                };

                var result = await _service.ActualizarCodigoAsync(dto);

                if (!result.Success)
                {
                    MessageBox.Show(result.Message ?? "No fue posible guardar", "Código Operacional",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _window.DialogResult = true;
                _window.Close();
            }
            finally
            {
                Guardando = false;
            }
        }
    }
}
