namespace APISegura.Dtos.Inspecciones
{
    public class InspeccionListDto
    {
        public long InspeccionId { get; set; }

        public DateTime FechaInspeccion { get; set; }

        public DateTime? FechaCorreo { get; set; }

        public string? NumeroCorreo { get; set; }

        public int PateTempor { get; set; }

        public int UsuarioId { get; set; }

        public DateTime created_at { get; set; }
    }
}
