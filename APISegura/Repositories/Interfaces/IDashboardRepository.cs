using Application.DTOs.Dashboard;

namespace Domain.Interfaces.Repositories;

public interface IDashboardRepository
{
    Task<IEnumerable<DashboardUltimoCuartelDto>> ObtenerUltimosCuartelesAsync();

    Task<IEnumerable<DashboardUltimaPlantaDto>> ObtenerUltimasPlantasAsync();

    Task<IEnumerable<DashboardCajasPorCodigoDto>> ObtenerCajasPorCodigoAsync();

    Task<IEnumerable<DashboardTotalCajasPorEspecieDto>> ObtenerTotalCajasHoyAsync();

    Task<int> ObtenerTotalPalletsHoyAsync();
}