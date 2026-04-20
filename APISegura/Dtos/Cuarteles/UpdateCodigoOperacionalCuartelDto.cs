namespace APISegura.Dtos.Cuarteles
{
    public class UpdateCodigoOperacionalCuartelDto
    {
        public int Productor { get; set; }
        public int Predio { get; set; }
        public int CodigoCuartel { get; set; }
        public string CodigoOperacional { get; set; } = null!;
    }
}
