using Estacionei.DTOs.ClienteVeiculo;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Estacionei.DTOs.Cliente
{
    public class ClienteRequestCreateDto
    {
        [Required(ErrorMessage = "Nome é obrigatorio")]
        [StringLength(50, ErrorMessage = "O nome deve ter no máximo 50 caracteres.")]
        public string ClienteNome { get; set; }

        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [Phone(ErrorMessage = "O telefone deve ser válido.")]
        public string ClienteTelefone { get; set; }

        public VeiculoClienteDto? Veiculo { get; set; }

    }
}
