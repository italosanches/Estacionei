using Estacionei.DTOs.ClienteVeiculo;
using Estacionei.DTOs.Veiculos;
using System.Text.Json.Serialization;

namespace Estacionei.DTOs.Cliente
{
    public class CustomerResponseDto
    {
        public int CustomerId { get; set; }

        public string CustomerName { get; set; }


        public string CustomerPhone { get; set; }

        public ICollection<CustomerVehicleResponseDto>? CustomerVehicles { get; set; }
    }
}
