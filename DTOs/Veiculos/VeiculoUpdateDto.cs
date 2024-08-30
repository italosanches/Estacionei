using Estacionei.Enums;
using System.ComponentModel.DataAnnotations;

namespace Estacionei.DTOs.Veiculos
{
	public class VeiculoUpdateDto
	{
		[Required(ErrorMessage = "É necessário inserir a placa.")]
		[Length(1, 15, ErrorMessage = "Placa deve ter entre 1 e 15 caracteres.")]
		public string VeiculoPlaca { get; set; }
		[Required]
		[Length(1, 10, ErrorMessage = "Modelo deve conter entre 1 e 10 caracteres.")]
		public string VeiculoModelo { get; set; }
		[Required(ErrorMessage = "Tipo necessario, 1= Carro, 2= Moto e 3=Camionete.")]
		public TipoVeiculo TipoVeiculo { get; set; }
		[Required(ErrorMessage = "Id do cliente dono do carro é obrigatorio.")]
		public int ClienteId { get; set; }
	}
}

