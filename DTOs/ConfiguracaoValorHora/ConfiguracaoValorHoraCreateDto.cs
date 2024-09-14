using Estacionei.Enums;
using Estacionei.Validations;
using System.ComponentModel.DataAnnotations;

namespace Estacionei.DTOs.ConfiguracaoValorHora
{
    public class ConfiguracaoValorHoraCreateDto
    {
        [Required (ErrorMessage ="Tipo de veiculo é obrigatorio")]
        [ValidateEnum(typeof(TipoVeiculo))]
        public TipoVeiculo TipoVeiculo { get; set; }

        [Required(ErrorMessage = "Valor por hora é obrigatorio.")]

        public decimal ValorHora { get; set; }
    }
}
