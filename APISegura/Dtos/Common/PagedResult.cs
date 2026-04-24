namespace APISegura.Dtos.Common
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalRegistros { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPaginas => (int)Math.Ceiling((double)TotalRegistros / PageSize);
    }
}
