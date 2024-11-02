using Estacionei.DTOs.Cliente;
using Estacionei.Enums;
using System.ComponentModel.DataAnnotations;

namespace Estacionei.DTOs.Veiculos
{
	public class VeiculoResponseDto
	{
        public int VeiculoId { get; set; }
        public string? VeiculoPlaca { get; set; }
		
		public string? VeiculoModelo { get; set; }
		
		public TipoVeiculo TipoVeiculo { get; set; }
		
		public string? ClienteNome {  get; set; }
		public int ClienteId { get; set; }
	}
}
