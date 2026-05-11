namespace Application.DTOs.Dashboard;

public class DashboardUltimaPlantaDto
{
    public string Codigo { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public string Usuario { get; set; } = string.Empty;

    public DateTime FechaActualizacion { get; set; }
}