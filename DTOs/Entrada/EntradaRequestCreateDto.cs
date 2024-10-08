﻿using Estacionei.Enums;
using Estacionei.Validations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Estacionei.DTOs.Entrada
{
    public class EntradaRequestCreateDto
    {
        [Required(ErrorMessage ="O ID do veiculo é obrigatorio.")]
        public int VeiculoId { get; set; }

        public DateTime DataEntrada { get; set; }

        [JsonIgnore]
        public StatusEntrada StatusEntrada { get; set; }
    }
}
