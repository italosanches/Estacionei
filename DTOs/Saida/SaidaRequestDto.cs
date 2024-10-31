using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Estacionei.DTOs.Saida
{
    public class SaidaRequestDto
    {
        [JsonIgnore]
        public int SaidaId { get;set; }

        [Required(ErrorMessage = "ID entrada é obrigatorio.")]
        public int EntradaId { get; set; }

        [Required(ErrorMessage = "Data de saida é obrigatorio.")]
        public DateTime DataSaida { get; set; }
    }
}
