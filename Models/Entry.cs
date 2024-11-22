using Estacionei.Enums;
using System.ComponentModel.DataAnnotations;

namespace Estacionei.Models
{
    public class Entry
    {
        [Required]
        public int EntryId { get; set; }

        [Required]
        public int VehicleId { get; set; }

        [Required]
        public DateTime EntryDate { get; set; }

        public EntryStatus EntryStatus { get; set; } = EntryStatus.Open;

        public virtual Vehicle Vehicle { get; set; }
        public virtual Exit Exit { get; set; }
    }

}
