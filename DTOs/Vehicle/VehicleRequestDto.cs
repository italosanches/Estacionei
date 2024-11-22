using Estacionei.Enums;
using Estacionei.Validations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Estacionei.DTOs.Vehicle
{
    public class VehicleRequestDto
    {
        [JsonIgnore]
        public int VehicleId { get; set; }

        [Required(ErrorMessage = "É necessário inserir a placa.")]
        [Length(1, 15, ErrorMessage = "Placa deve ter entre 1 e 15 caracteres.")]
        public string? VehicleLicensePlate { get; set; }
        [Required]
        [Length(1, 10, ErrorMessage = "Modelo deve conter entre 1 e 10 caracteres.")]
        public string? VehicleModel { get; set; }
        [Required(ErrorMessage = "Tipo necessario, 1= Carro, 2= Moto e 3=Camionete.")]
        [ValidateEnum(typeof(VehicleType))]
        public VehicleType VehicleType { get; set; }
        [Required(ErrorMessage = "Id do cliente dono do carro é obrigatorio.")]
        public int CustomerId { get; set; }
    }
}
