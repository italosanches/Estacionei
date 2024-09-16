using Estacionei.Enums;
using Estacionei.Validations;
using System.ComponentModel.DataAnnotations;

namespace Estacionei.DTOs.Entrada
{
    public class EntradaResponseDto
    {
        
        public int EntradaId { get; set; }

        
        public int VeiculoId { get; set; }
       

        public DateTime DataEntrada { get; set; }

      
        public StatusEntrada StatusEntrada { get; set; }
    }
}
