using System.ComponentModel.DataAnnotations;

namespace Estacionei.DTOs.Authentication
{
    public class LoginDto
    {

        [Required(ErrorMessage = "E-mail é obrigatorio.")]
        [EmailAddress (ErrorMessage ="Insira um e-mail valido!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha é obrigatoria.")]

        public string Password { get; set; }
    }
}
