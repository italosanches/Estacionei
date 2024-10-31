namespace Estacionei.DTOs.Saida
{
    public class SaidaResponseDto
    {
        public int SaidaId { get; set; }
        public int EntradaId { get; set; }

        public DateTime DataEntrada { get; set; }
        public DateTime DataSaida { get; set; }

        public decimal ValorCobrado { get; set; }

        public int VeiculoId {  get; set; }
        public string? VeiculoPlaca { get; set; }
        public string VeiculoModelo { get; set; }
        public int ClienteId { get; set; }
        public string? ClienteNome { get; set; }

    }
}
