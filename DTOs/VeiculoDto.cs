using Estacionei.Enums;

namespace Estacionei.DTOs
{
	public class VeiculoDto
	{
		public string VeiculoPlaca { get; set; }
		public string VeiculoModelo { get; set; }
		public TipoVeiculo TipoVeiculo { get; set; }
		public int ClienteId { get; set; }
	}
}
