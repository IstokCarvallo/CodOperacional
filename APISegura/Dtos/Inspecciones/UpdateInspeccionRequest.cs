namespace APISegura.Dtos.Inspecciones
{
    public class UpdateInspeccionRequest
    {
        public DateTime FechaInspeccion { get; set; }

        public DateTime? FechaCorreo { get; set; }

        public string? NumeroCorreo { get; set; }

        public int PateTempor { get; set; }

        public List<CreateInspeccionFolioRequest> Folios { get; set; }
            = new();

        public List<int> EtiquetasTAG { get; set; }
            = new();
    }
}
