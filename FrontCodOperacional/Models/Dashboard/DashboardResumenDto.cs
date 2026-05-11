namespace FrontCodOperacional.Models.Dashboard;

public class DashboardResumenDto
{
    public IEnumerable<DashboardUltimoCuartelDto>UltimosCuarteles { get; set; } = Enumerable.Empty<DashboardUltimoCuartelDto>();

    public IEnumerable<DashboardUltimaPlantaDto>UltimasPlantas { get; set; } = Enumerable.Empty<DashboardUltimaPlantaDto>();

    public IEnumerable<DashboardCajasPorCodigoDto>CajasPorCodigo { get; set; } = Enumerable.Empty<DashboardCajasPorCodigoDto>();

    public IEnumerable<DashboardTotalCajasPorEspecieDto>TotalCajasHoy { get; set; } = Enumerable.Empty<DashboardTotalCajasPorEspecieDto>();

    public int TotalPalletsHoy { get; set; }
}