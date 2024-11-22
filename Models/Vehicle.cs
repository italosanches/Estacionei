using Estacionei.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Estacionei.Models
{
    [Table("Vehicles")]
    public class Vehicle
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VehicleId { get; set; }

        [Required]
        [StringLength(15)]
        public string? VehicleLicensePlate { get; set; }

        [Required]
        [StringLength(11)]
        public required string VehicleModel { get; set; }

        [Required]
        public VehicleType VehicleType { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public virtual Customer? Customer { get; set; }
    }
}
