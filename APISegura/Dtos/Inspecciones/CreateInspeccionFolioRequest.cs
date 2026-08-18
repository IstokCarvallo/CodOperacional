namespace APISegura.Dtos.Inspecciones
{
    public class CreateInspeccionFolioRequest
    {
        public int ClieCodigo { get; set; }

        public int PldeCodigo { get; set; }

        public int PaenNumero { get; set; }

        public int EspeCodigo { get; set; }

        public int DefeNumero { get; set; }

        public byte Nota { get; set; }

        public bool EsSupermercado { get; set; }

        public int? Causal1Id { get; set; }
        public int? Causal2Id { get; set; }
        public int? Causal3Id { get; set; }

        public string? Observacion { get; set; }

        public decimal? PromedioFirmeza { get; set; }

        public decimal? PromedioBrix { get; set; }
    }
}
