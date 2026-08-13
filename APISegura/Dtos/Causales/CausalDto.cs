namespace APISegura.Dtos.Causales
{
    public class CausalDto
    {
        public int Causal_Id { get; set; }
        public int Codigo { get; set; }
        public string Nombre{ get; set; } = string.Empty;
        public bool Activo { get; set; }
    }
}
