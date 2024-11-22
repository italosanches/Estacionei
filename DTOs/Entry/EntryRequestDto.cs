using Estacionei.Enums;
using Estacionei.Validations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Estacionei.DTOs.Entrada
{
    public class EntryRequestDto
    {
        [JsonIgnore]
        public int EntryId { get; set; }

        [Required(ErrorMessage ="O ID do veiculo é obrigatorio.")]
        public int VehicleId { get; set; }

        [Required(ErrorMessage ="Data de entrada é obrigatorio.")]
        public DateTime EntryDate { get; set; }

        [JsonIgnore]
        public EntryStatus EntryStatus { get; set; }
    }
}
