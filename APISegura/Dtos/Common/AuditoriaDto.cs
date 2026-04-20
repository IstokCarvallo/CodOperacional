namespace APISegura.Dtos.Common
{
    public class AuditoriaDto
    {
        public long Id { get; set; }

        public string Entidad { get; set; }
        public string Accion { get; set; }
        public string Clave { get; set; }

        public string Campo { get; set; }
        public string? ValorAnterior { get; set; }
        public string? ValorNuevo { get; set; }

        public string Usuario { get; set; }
        public DateTime Fecha { get; set; }
    }
}
