using Estacionei.Models;

namespace Estacionei.Repository.Interfaces
{
    public interface ISaidaRepository : IRepository<Saida>
    {
        public Task<decimal> SearchVehicleAndCalculateTheAmountPay(int veiculoId, DateTime dataEntrada, DateTime dataSaida);
    }
}
