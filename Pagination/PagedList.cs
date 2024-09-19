using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Estacionei.Pagination
{
    public class PagedList<TResult> : List<TResult> where TResult : class
    {
      

        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize  { get; private set; }
        public int TotalCount { get; private set; }

        public bool HasPrevious { get { return CurrentPage >1; } }
        public bool HasNext => CurrentPage < TotalPages;

        public PagedList(List<TResult> items,int count,int currentPage, int pageSize)
        {
            CurrentPage = currentPage;
            TotalPages = (int)Math.Ceiling(count/(double)pageSize);
            PageSize = pageSize;
            TotalCount = count;
            AddRange(items);
        }

        public static async Task<PagedList<TResult>> CreateAsync<TSource>(IQueryable<TSource> source,
                                                           int currentPage,int pageSize, IMapper mapper)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappedItems = items.Select(item => mapper.Map<TResult>(item)).ToList(); // Mapeia os itens
            return new PagedList<TResult>(mappedItems, count, currentPage, pageSize);
        }

    }
}

