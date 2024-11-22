using Estacionei.Enums;
using Estacionei.Validations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Estacionei.DTOs.ConfiguracaoValorHora
{
    public class HourPriceConfigurationRequestDto
    {
        [JsonIgnore]
        public int HourPriceConfigurationId { get; set; }

        [Required (ErrorMessage ="Tipo de veiculo é obrigatorio")]
        [ValidateEnum(typeof(VehicleType))]
        public VehicleType VehicleType { get; set; }

        [Required(ErrorMessage = "Valor por hora é obrigatorio.")]

        public decimal HourlyRate { get; set; }
    }
}
