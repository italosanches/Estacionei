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

        [Required(ErrorMessage ="Nome é obrigatorio.")]
        [Length(1,50,ErrorMessage ="Nome deve ter entre 1 e 50 caracteres.")]
        [StringLength(50)]
        public string ClienteNome { get; set; }

        [MinLength(11,ErrorMessage ="Telefone invalido.")]
        [StringLength(12)]
        public string ClienteTelefone {  get; set; } 

        public ICollection<Veiculo> VeiculosCliente { get; set; }
    }
}
