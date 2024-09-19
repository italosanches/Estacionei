namespace Estacionei.Pagination.Parameters
{
    public class QueryParameters
    {
        const int _maxPageSize = 50;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = _maxPageSize ;
        public int PageSize 
        { 
            get { return _pageSize; } 
            set{ _pageSize = (value > _maxPageSize) ? _maxPageSize : value; } 
        }
    }
}
