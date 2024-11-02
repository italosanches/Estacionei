using Estacionei.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Estacionei.DTOs.ConfiguracaoValorHora
{
    public class ConfiguracaoValorHoraResponseDto 
    {
        public int Id { get; set; }
        public TipoVeiculo TipoVeiculo { get; set; }
        public decimal ValorHora { get; set; }
    }
}
