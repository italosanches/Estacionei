using System.ComponentModel.DataAnnotations.Schema;

namespace Estacionei.Models
{
    public class Exit
    {
        public int ExitId { get; set; }
        public int EntryId { get; set; }

        public DateTime ExitDate { get; set; } = DateTime.MinValue;

        [Column(TypeName = "decimal(10,2)")]
        public decimal ChargedAmount { get; set; }

        public virtual Entry Entry { get; set; }
    }

}
