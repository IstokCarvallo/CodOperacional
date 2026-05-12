namespace DesktopCodOperacional.Models.Cuartel
{
    public class UpdateCodigoOperacionalDto
    {
        public int Productor { get; set; }

        public int Predio { get; set; }

        public int CodigoCuartel { get; set; }

        public string CodigoOperacional { get; set; } = string.Empty;
    }
}
