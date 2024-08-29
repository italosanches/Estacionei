using System.ComponentModel.DataAnnotations;

namespace Estacionei.DTOs.Cliente
{
	public class ClienteUpdateDto
	{
		[Required]
		public int ClienteId { get; set; }

		[Required(ErrorMessage = "Nome é obrigatorio")]
		[StringLength(50, ErrorMessage = "O nome deve ter no máximo 50 caracteres.")]
		public string ClienteNome { get; set; }

		[Required(ErrorMessage = "O telefone é obrigatório.")]
		[Phone(ErrorMessage = "O telefone deve ser válido.")]
		public string ClienteTelefone { get; set; }
	}
}
