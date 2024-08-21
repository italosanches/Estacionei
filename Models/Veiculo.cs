using Estacionei.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Estacionei.Models
{
    [Table("Veiculos")]
    public class Veiculo
    {
        [Key]
        [StringLength(15)]
        public string VeiculoPlaca { get; set; }

        [Required]
        [Length(1, 10, ErrorMessage = "Modelo deve conter entre 1 e 10 caracteres")]
        [StringLength(11)]
        public string VeiculoModelo { get; set; }
        [Required]
        public TipoVeiculo TipoVeiculo { get; set; }

        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
    }
}
