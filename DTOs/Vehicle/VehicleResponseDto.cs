using Estacionei.DTOs.Cliente;
using Estacionei.Enums;
using System.ComponentModel.DataAnnotations;

namespace Estacionei.DTOs.Veiculos
{
	public class VehicleResponseDto
	{
        public int VehicleId { get; set; }
        public string? VehicleLicensePlate { get; set; }
		
		public string? VehicleModel { get; set; }
		
		public VehicleType VehicleType { get; set; }
		
		public string? CustomerName {  get; set; }
		public int CustomerId { get; set; }
	}
}
