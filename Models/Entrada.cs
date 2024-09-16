using Estacionei.Enums;
using System.ComponentModel.DataAnnotations;

namespace Estacionei.Models
{
    public class Entrada
    {
        [Required]
        public int EntradaId { get; set; }

        [Required]
        public int VeiculoId { get; set; }
        [Required(ErrorMessage ="Data de entrada é obrigatorio.")]
        
        public DateTime? DataEntrada { get; set; }

        public StatusEntrada StatusEntrada { get; set; } =  StatusEntrada.Aberto;

        public virtual Veiculo Veiculo { get; set; }
        public virtual Saida Saida { get; set; }
    }
}
