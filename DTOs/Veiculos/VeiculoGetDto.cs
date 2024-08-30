using Estacionei.DTOs.Cliente;
using Estacionei.Enums;
using System.ComponentModel.DataAnnotations;

namespace Estacionei.DTOs.Veiculos
{
	public class VeiculoGetDto
	{
		public string VeiculoPlaca { get; set; }
		
		public string VeiculoModelo { get; set; }
		
		public TipoVeiculo TipoVeiculo { get; set; }
		
		public ClienteGetDto Cliente { get; set; }
	}
}
