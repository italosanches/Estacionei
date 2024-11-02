using AutoMapper;
using Estacionei.Pagination;
using Estacionei.Pagination.Parameters;

namespace Estacionei.Services
{
    public class PagedListService<TResult,TSource> where TResult : class where TSource : class
    {
        public static async Task<PagedList<TResult>> CreatePagedList(IQueryable<TSource>source,QueryParameters parameters,IMapper mapper)
        {
            return await PagedList<TResult>.CreateAsync(source, parameters.PageNumber, parameters.PageSize, mapper);
        }
    }
}
