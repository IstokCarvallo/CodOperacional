namespace APISegura.Dtos.Inspecciones
{
    public class InspeccionEtiquetaTAGDto
    {
        public int EtiquetaId { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }
    }
}
