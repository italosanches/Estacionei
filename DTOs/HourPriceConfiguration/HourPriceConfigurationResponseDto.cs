using Estacionei.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Estacionei.DTOs.ConfiguracaoValorHora
{
    public class HourPriceConfigurationResponseDto 
    {
        public int HourPriceConfigurationId { get; set; }
        public VehicleType VehicleType { get; set; }
        public decimal HourlyRate { get; set; }
    }
}
