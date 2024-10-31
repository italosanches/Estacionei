using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Estacionei.DTOs.Saida
{
    public class SaidaUpdateRequestDto
    {
        [JsonIgnore]
        public int SaidaId { get; set; }
        [Required(ErrorMessage = "Data de saida é obrigatorio.")]
        public DateTime DataSaida { get; set; }
    }
}
