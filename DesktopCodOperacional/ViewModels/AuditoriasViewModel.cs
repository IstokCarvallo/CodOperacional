using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopCodOperacional.Models.Auditoria;
using DesktopCodOperacional.Services.Api;
using DesktopCodOperacional.Services.UI;
using DesktopCodOperacional.ViewModels.Base;
using System.Collections.ObjectModel;

namespace DesktopCodOperacional.ViewModels
{
    public partial class AuditoriasViewModel : OperationalViewModel
    {
        private readonly AuditoriaService _service;

        private readonly NotificationService _notification;

        [ObservableProperty]
        private ObservableCollection<AuditoriaDto> auditorias = [];

        [ObservableProperty]
        private AuditoriaDto? selectedAuditoria;

        [ObservableProperty]
        private DateTime? desde;

        [ObservableProperty]
        private DateTime? hasta;

        [ObservableProperty]
        private string? usuario;

        [ObservableProperty]
        private string? entidad;

        [ObservableProperty]
        private string? accion;

        [ObservableProperty]
        private bool isLoading;

        public AuditoriasViewModel(AuditoriaService service, NotificationService notification)
        {
            _service = service;
            _notification = notification;
        }

        public async Task InicializarAsync()
        {
            await BuscarAsync();
        }

        [RelayCommand]
        private async Task BuscarAsync()
        {
            try
            {
                IsLoading = true;

                var resultado = await _service.BuscarAsync(Desde, Hasta, Usuario, Entidad, Accion);

                Auditorias = new ObservableCollection<AuditoriaDto>(
                    resultado.OrderByDescending(x => x.Fecha));
            }
            catch (Exception ex)
            {
                _notification.Error($"Error al cargar auditoría: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task LimpiarAsync()
        {
            Desde =null;
            Hasta = null;
            Usuario = null;
            Entidad = null;
            Accion = null;

            await BuscarAsync();
        }

        [RelayCommand]
        private async Task RefrescarAsync()
        {
            await BuscarAsync();
        }

        [RelayCommand]
        private async Task VerDetalleAsync()
        {
            if (SelectedAuditoria is null)
            {
                _notification.Warning("Seleccione un registro.");
                return;
            }

            var auditoria = await _service.ObtenerPorIdAsync(
                SelectedAuditoria.Id);

            if (auditoria is null)
            {
                _notification.Error("No fue posible recuperar el detalle.");
                return;
            }

            // Lo conectaremos después con la ventana detalle
        }
    }
}
