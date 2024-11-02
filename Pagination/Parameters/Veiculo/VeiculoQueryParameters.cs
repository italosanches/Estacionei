using Estacionei.Enums;

namespace Estacionei.Pagination.Parameters.Veiculo
{
    public class VeiculoQueryParameters : QueryParameters
    {
        public int ClienteId { get; set; }
        public string? ClienteNome { get; set; }
        public TipoVeiculo TipoVeiculo { get; set; }
    }
}
