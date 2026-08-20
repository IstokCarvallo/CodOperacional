namespace APISegura.Dtos.Inspecciones
{
    public class InspeccionDto
    {
        public long InspeccionId { get; set; }

        public DateTime FechaInspeccion { get; set; }

        public DateTime? FechaCorreo { get; set; }

        public string? NumeroCorreo { get; set; }

        public int PateTempor { get; set; }

        public int UsuarioId { get; set; }

        public DateTime created_at { get; set; }

        public DateTime? updated_at { get; set; }

        public List<InspeccionFolioDto> Folios { get; set; }
            = new();

        public List<InspeccionEtiquetaTAGDto> EtiquetasTAG { get; set; }
            = new();
    }
}
