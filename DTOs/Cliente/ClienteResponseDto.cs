using Estacionei.DTOs.ClienteVeiculo;
using Estacionei.DTOs.Veiculos;
using System.Text.Json.Serialization;

namespace Estacionei.DTOs.Cliente
{
    public class ClienteResponseDto
    {
        public int ClienteId { get; set; }

        public string ClienteNome { get; set; }


        public string ClienteTelefone { get; set; }

        public ICollection<VeiculoClienteDto>? VeiculosCliente { get; set; }
    }
}
