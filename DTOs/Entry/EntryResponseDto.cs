using Estacionei.DTOs.Veiculos;
using Estacionei.Enums;
using Estacionei.Validations;
using System.ComponentModel.DataAnnotations;

namespace Estacionei.DTOs.Entrada
{
    public class EntryResponseDto
    { 
        public int EntryId { get; set; }

        public int    VehicleId { get; set; }
        public string? VehicleLicensePlate { get; set; }
        public string? VehicleModel { get; set; }
      
       
        public int CustomerId { get; set; } 
        public string? CustomerName { get; set; }
        public DateTime EntryDate { get; set; }

      
        public EntryStatus EntryStatus { get; set; }
    }
}
