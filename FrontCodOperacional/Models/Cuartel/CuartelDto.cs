namespace FrontCodOperacional.Models.Cuartel
{
    public class CuartelDto
    {
        public int Productor { get; set; }
        public int Predio { get; set; }
        public int CodigoCuartel { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? CodigoOperacional { get; set; }
        public string Especie { get; set; } = string.Empty;
        public string Variedad { get; set; } = string.Empty;
        public DateTime? FechaUltimaActualizacion { get; set; }
    }
}
