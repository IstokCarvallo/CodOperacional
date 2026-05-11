namespace Application.DTOs.Dashboard;

public class DashboardCajasPorCodigoDto
{
    public string Especie { get; set; } = string.Empty;

    public string CodigoOperacional { get; set; } = string.Empty;

    public int CantidadCajas { get; set; }
}