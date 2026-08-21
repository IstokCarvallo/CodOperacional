namespace FrontCodOperacional.Models.Inspeccion
{
    public class InspeccionFiltroRequest
    {
        public DateTime? FechaInspeccionDesde { get; set; }
        public DateTime? FechaInspeccionHasta { get; set; }

        public DateTime? FechaCorreoDesde { get; set; }
        public DateTime? FechaCorreoHasta { get; set; }

        public string? NumeroCorreo { get; set; }

        public int? PateTempor { get; set; }

        public long? InspeccionId { get; set; }
    }
}
