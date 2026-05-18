namespace DesktopCodOperacional.Models.Planta
{
    public class PagedResponse<T>
    {
        public List<T> Items { get; set; } = [];

        public int TotalRegistros { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalPaginas { get; set; }
    }
}
