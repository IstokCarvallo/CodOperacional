using Application.DTOs.Dashboard;

namespace APISegura.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<IEnumerable<DashboardUltimoCuartelDto>> ObtenerUltimosCuartelesAsync();

        Task<IEnumerable<DashboardUltimaPlantaDto>> ObtenerUltimasPlantasAsync();

        Task<IEnumerable<DashboardCajasPorCodigoDto>> ObtenerCajasPorCodigoAsync();

        Task<IEnumerable<DashboardTotalCajasPorEspecieDto>> ObtenerTotalCajasHoyAsync();

        Task<int> ObtenerTotalPalletsHoyAsync();

        Task<DashboardResumenDto> ObtenerResumenAsync();
    }
}
