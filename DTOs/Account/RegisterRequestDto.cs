using System.ComponentModel.DataAnnotations;

namespace Estacionei.DTOs.Authentication
{
    public class RegisterRequestDto
    {
        [Required(ErrorMessage = "Login é obrigatorio.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Senha é obrigatoria.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "E-mail é obrigatorio.")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
