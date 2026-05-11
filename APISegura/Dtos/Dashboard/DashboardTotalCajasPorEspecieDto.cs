namespace Application.DTOs.Dashboard;

public class DashboardTotalCajasPorEspecieDto
{
    public string Especie { get; set; } = string.Empty;

    public int TotalCajasHoy { get; set; }
}