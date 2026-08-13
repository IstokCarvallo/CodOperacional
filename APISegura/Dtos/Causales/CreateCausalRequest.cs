namespace APISegura.Dtos.Causales
{
    public class CreateCausalRequest
    {
        public int Codigo { get; set; }
        public string Nombre{ get; set; } = string.Empty;
    }
}
