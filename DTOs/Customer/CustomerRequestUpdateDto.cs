using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Estacionei.DTOs.Customer
{
	public class CustomerRequestUpdateDto
    {
		[JsonIgnore]
		public int CustomerId { get; set; }

		[Required(ErrorMessage = "Nome é obrigatorio")]
		[StringLength(50, ErrorMessage = "O nome deve ter no máximo 50 caracteres.")]
		public string CustomerName { get; set; }

		[Required(ErrorMessage = "O telefone é obrigatório.")]
		[Phone(ErrorMessage = "O telefone deve ser válido.")]
		public string CustomerPhone { get; set; }
	}
}
