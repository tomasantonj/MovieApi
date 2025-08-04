using System.Collections.Generic;

namespace Movie.Contracts
{
    public class PagedResponse<T>
    {
        public IEnumerable<T> Data { get; set; }
        public PagedMeta Meta { get; set; }
    }
    public class PagedMeta
    {
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
    }
}
