namespace APISegura.Dtos.Cuarteles
{
    public class CuartelDto
    {
        public int Productor { get; set; }
        public int Predio { get; set; }
        public int CodigoCuartel { get; set; }
        public string Nombre { get; set; } = null!;
        public string CodigoOperacional { get; set; } = null!;
        public DateTime? FechaUltimaActualizacion { get; set; }
    }
}
