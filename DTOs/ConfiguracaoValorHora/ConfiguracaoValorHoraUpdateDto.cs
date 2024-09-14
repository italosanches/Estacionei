using Estacionei.Enums;
using Estacionei.Validations;
using System.Text.Json.Serialization;

namespace Estacionei.DTOs.ConfiguracaoValorHora
{
    public class ConfiguracaoValorHoraUpdateDto 
    {
        [JsonIgnore]
        public int Id { get; set; }

        [ValidateEnum(typeof(TipoVeiculo))]
        public TipoVeiculo TipoVeiculo { get; set; }
        public decimal ValorHora { get; set; }
    }
}
