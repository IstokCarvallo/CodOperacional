namespace DesktopCodOperacional.Models.Auditoria
{
    public class AuditoriaDto
    {
        public int Id { get; set; }

        public string Entidad { get; set; } = string.Empty;

        public string Accion { get; set; } = string.Empty;

        public string Clave { get; set; } = string.Empty;

        public string Campo { get; set; } = string.Empty;

        public string? ValorAnterior { get; set; }

        public string? ValorNuevo { get; set; }

        public string Usuario { get; set; } = string.Empty;

        public DateTime Fecha { get; set; }
    }
}
