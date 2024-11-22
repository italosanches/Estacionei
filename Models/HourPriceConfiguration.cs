using Estacionei.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Estacionei.Models
{
    public class HourPriceConfiguration
    {
        public int HourPriceConfigurationId { get; set; }
        public VehicleType VehicleType { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal HourlyRate { get; set; }
    }

}
