﻿using Estacionei.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Estacionei.Models
{
    [Table("Veiculos")]
    public class Veiculo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VeiculoId { get; set; }

        [Required(ErrorMessage = "É necessário inserir a placa")]
        [StringLength(15)]
        public string? VeiculoPlaca { get; set; }

        [Required]
        [Length(1, 10, ErrorMessage = "Modelo deve conter entre 1 e 10 caracteres")]
        [StringLength(11)]
        public required string VeiculoModelo { get; set; }

        [Required(ErrorMessage ="Tipo necessario, 1= Carro, 2= Moto e 3=Camionete")]
        public TipoVeiculo TipoVeiculo { get; set; }
        [Required(ErrorMessage ="ID do cliente é obrigatorio.")]
        public int ClienteId { get; set; }

        public virtual Cliente? Cliente { get; set; }
    }
}
