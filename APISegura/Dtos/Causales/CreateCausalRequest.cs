namespace APISegura.Dtos.Causales
{
    public class CreateCausalRequest
    {
        public int EspeCodigo { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public byte Tipo { get; set; }
    }
}
