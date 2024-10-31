namespace Estacionei.Pagination.Parameters.EntradaParameters
{
    public class EntradaQueryParameters : QueryParameters
    {
        private DateTime _dataInicio = DateTime.MinValue;
        private DateTime _dataFim = DateTime.MinValue;

        public DateTime DataInicio { get { return _dataInicio; } set { _dataInicio = value; } }
        public DateTime DataFim { get { return _dataFim; } set { _dataFim = value; } }

        public int VeiculoId { get; set; }
        public string? VeiculoPlaca { get; set; }


    }
}

