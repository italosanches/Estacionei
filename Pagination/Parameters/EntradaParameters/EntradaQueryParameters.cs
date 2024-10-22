namespace Estacionei.Pagination.Parameters.EntradaParameters
{
    public class EntradaQueryParameters : QueryParameters
    {
        private DateTime _dataInicio = DateTime.MinValue;
        private DateTime _dataFim = DateTime.UtcNow;

        public DateTime DataInicio { get { return _dataInicio; } set { _dataInicio = value; } }
        public DateTime DataFim { get { return _dataFim; } set { _dataFim = value; } }




    }
}

