namespace APISegura.Dtos.Causales
{
    public class CausalDto
    {
        public int CausalId { get; set; }
        public int EspeCodigo { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public byte Tipo { get; set; }
        public bool Activo { get; set; }
    }
}
