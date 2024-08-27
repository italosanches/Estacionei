using System.ComponentModel.DataAnnotations;

namespace Estacionei.DTOs.Cliente
{
	public class ClienteUpdateDto
	{
		[Required]
		public int ClienteId { get; set; }
		public string ClienteNome { get; set; }
		public string ClienteTelefone { get; set; }
	}
}
