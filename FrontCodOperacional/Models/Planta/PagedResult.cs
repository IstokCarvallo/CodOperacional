namespace FrontCodOperacional.Models.Planta
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalRegistros { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPaginas { get; set; }
    }
}
