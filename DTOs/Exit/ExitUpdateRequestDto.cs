using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Estacionei.DTOs.Saida
{
    public class ExitUpdateRequestDto
    {
        [JsonIgnore]
        public int ExitId { get; set; }
        [Required(ErrorMessage = "Data de saida é obrigatorio.")]
        public DateTime ExitDate { get; set; }
    }
}
