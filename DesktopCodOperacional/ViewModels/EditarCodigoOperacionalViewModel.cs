using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopCodOperacional.Models.Cuartel;
using DesktopCodOperacional.Services.Api;
using DesktopCodOperacional.Services.UI;
using DesktopCodOperacional.ViewModels.Base;
using System.Windows;

namespace DesktopCodOperacional.ViewModels
{
    public partial class EditarCodigoOperacionalViewModel : BaseViewModel
    {
        private readonly NotificationService _notification;
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

        public EditarCodigoOperacionalViewModel(NotificationService notification,
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
            _notification = notification;

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
                _notification.Info("Debe ingresar el código operacional");
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
                    _notification.Error(result.Message ?? "No fue posible guardar");
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
