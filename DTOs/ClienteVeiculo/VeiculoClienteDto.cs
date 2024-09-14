using Estacionei.Enums;
using Estacionei.Validations;
using System.ComponentModel.DataAnnotations;

namespace Estacionei.DTOs.ClienteVeiculo
{
    public class VeiculoClienteDto
    {
        [Required(ErrorMessage = "É necessário inserir a placa.")]
        [Length(1, 15, ErrorMessage = "Placa deve ter entre 1 e 15 caracteres.")]
        public required string VeiculoPlaca { get; set; }
        [Required]
        [Length(1, 10, ErrorMessage = "Modelo deve conter entre 1 e 10 caracteres.")]
        public required string VeiculoModelo { get; set; }
        [Required(ErrorMessage = "Tipo necessario, 1= Carro, 2= Moto e 3=Camionete.")]
        [ValidateEnum(typeof(TipoVeiculo))]
        public TipoVeiculo TipoVeiculo { get; set; }

    }
}
