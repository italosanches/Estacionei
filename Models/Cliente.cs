using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Estacionei.Models
{
    [Table("Clientes")]
    public class Cliente
    {
        public Cliente() 
        { 
            VeiculosCliente = new Collection<Veiculo>(); 
        } 
        [Key]
        public int ClienteId { get; set; }

        [Required]
        [StringLength(50)]
        public string ClienteNome { get; set; }

        [Required]
        [Phone]
        public string ClienteTelefone {  get; set; } 

        public ICollection<Veiculo> VeiculosCliente { get; set; }
    }
}
