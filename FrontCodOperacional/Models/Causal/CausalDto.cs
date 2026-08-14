namespace FrontCodOperacional.Models.Causal
{
    public class CausalDto
    {
        public int CausalId { get; set; }
        public int Codigo { get; set; }
        public string Especie { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }
}
