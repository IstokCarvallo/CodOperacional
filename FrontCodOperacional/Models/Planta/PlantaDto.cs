namespace FrontCodOperacional.Models.Planta
{
    public class PlantaDto
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? CodigoOperacional { get; set; }
        public DateTime? FechaUltimaActualizacion { get; set; }
    }
}
