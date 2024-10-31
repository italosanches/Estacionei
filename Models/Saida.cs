using System.ComponentModel.DataAnnotations.Schema;

namespace Estacionei.Models
{
    public class Saida
    {
        public int SaidaId { get; set; }
        public int EntradaId { get; set; }

        public DateTime DataSaida { get; set; } = DateTime.MinValue;

        [Column(TypeName ="decimal(10,2)")]
        public decimal ValorCobrado {  get; set; }
        public virtual Entrada Entrada { get; set; }
    }
}
