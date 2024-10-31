using Estacionei.Context;
using Estacionei.Enums;
using Estacionei.Models;
using Estacionei.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Estacionei.Repository
{
    public class SaidaRepository : Repository<Saida>, ISaidaRepository
    {
        public SaidaRepository(AppDbContext context) : base(context)
        {
        }
        
        public async Task<decimal> SearchVehicleAndCalculateTheAmountPay(int veiculoId, DateTime dataEntrada, DateTime dataSaida)
        {
            var TipoveiculoProcurado = (await _context.Veiculos
                                              .AsNoTracking()
                                              .Where(veiculoProcurado => veiculoProcurado.VeiculoId == veiculoId)
                                              .FirstOrDefaultAsync())?.TipoVeiculo;

            var valorACobrar = (await _context.ConfiguracoesValoresHora
                               .AsNoTracking()
                               .Where(confValorHora => confValorHora.TipoVeiculo == TipoveiculoProcurado)
                               .FirstOrDefaultAsync())?.ValorHora;

            if (valorACobrar == null)
            {
               throw new NullReferenceException(message: "Não há configuração de valor hora, cadastre antes de continuar.");
            }

            TimeSpan horas = dataSaida - dataEntrada;
            double totalDeHoras = Math.Round(horas.TotalHours, MidpointRounding.AwayFromZero);

            return Math.Round(valorACobrar * (decimal)totalDeHoras?? 0, 2);
        }
    }
}
