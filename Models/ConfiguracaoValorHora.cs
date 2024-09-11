using Estacionei.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Estacionei.Models
{
    public class ConfiguracaoValorHora
    {
        public int Id { get; set; }
        public TipoVeiculo TipoVeiculo { get; set; }

        [Column(TypeName ="decimal(10,2)")]
        public decimal ValorHora { get; set; }
    }
}
