namespace APISegura.Dtos.Planta
{
    public class PlantaDto
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; } = null!;
        public string CodigoOperacional { get; set; } = null!;
        public DateTime? FechaUltimaActualizacion { get; set; }
    }
}
