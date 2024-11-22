using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Estacionei.Models
{
    [Table("Customers")]
    public class Customer
    {
        public Customer()
        {
            CustomerVehicles = new Collection<Vehicle>();
        }

        [Key]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(50)]
        public string CustomerName { get; set; }

        [Required]
        [Phone]
        public string CustomerPhone { get; set; }

        public ICollection<Vehicle> CustomerVehicles { get; set; }
    }
}
