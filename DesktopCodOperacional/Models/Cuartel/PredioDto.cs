namespace DesktopCodOperacional.Models.Cuartel
{
    public class PredioDto
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Display
        {
            get => $"{Codigo} - {Nombre}";
        }
    }
}