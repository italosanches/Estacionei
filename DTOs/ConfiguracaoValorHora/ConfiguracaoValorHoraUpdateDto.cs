using Estacionei.Enums;

namespace Estacionei.DTOs.ConfiguracaoValorHora
{
    public class ConfiguracaoValorHoraUpdateDto 
    {
        public int Id { get; set; }
        public TipoVeiculo TipoVeiculo { get; set; }
        public decimal ValorHora { get; set; }
    }
}
