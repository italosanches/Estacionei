namespace Estacionei.Pagination
{
    public class PaginationMetadata<T> where T : class
    {
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }
        public static PaginationMetadata<T> CreatePaginationMetadata(PagedList<T> pagedlist)
        {
            return new PaginationMetadata<T>
            {
                CurrentPage = pagedlist.CurrentPage,
                HasNext = pagedlist.HasNext,
                HasPrevious = pagedlist.HasPrevious,
                PageSize = pagedlist.PageSize,
                TotalCount = pagedlist.TotalCount,
                TotalPages = pagedlist.TotalPages
            };
        }
    }
}
