using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopCodOperacional.Models.Planta;
using DesktopCodOperacional.Services.Api;
using DesktopCodOperacional.Services.UI;
using System.Windows;

namespace DesktopCodOperacional.ViewModels
{
    public partial class EditarCodigoOperacionalPlantaViewModel : ObservableObject
    {
        private readonly NotificationService _notification;
        private readonly PlantaService _service;
        private readonly Window _window;

        [ObservableProperty]
        private int codigo;

        [ObservableProperty]
        private string nombre = string.Empty;

        [ObservableProperty]
        private string codigoOperacional = string.Empty;

        public EditarCodigoOperacionalPlantaViewModel(
            NotificationService notification,
            PlantaDto planta,
            PlantaService service,
            Window window)
        {
            _notification = notification;
            _service = service;
            _window = window;

            Codigo = planta.Codigo;
            Nombre = planta.Nombre;
            CodigoOperacional = planta.CodigoOperacional ?? "";
        }

        partial void OnCodigoOperacionalChanged(string value)
        {
            CodigoOperacional = value?.ToUpper() ?? "";
        }

        [RelayCommand]
        private void Cancelar()
        {
            _window.DialogResult = false;
            _window.Close();
        }

        [RelayCommand]
        private async Task Guardar()
        {
            var result = await _service.ActualizarCodigoAsync(
                new UpdateCodigoOperacionalPlantaDto
                {
                    Codigo = Codigo,
                    CodigoOperacional = CodigoOperacional
                });

            if (!result.Success)
            {
                _notification.Warning(result.Message);
                return;
            }

            _notification.Success(result.Message);

            _window.DialogResult = true;
            _window.Close();
        }
    }
}