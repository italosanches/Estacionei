using Estacionei.Models;

namespace Estacionei.Repository.Interfaces
{
    public interface IExitRepository : IRepository<Exit>
    {
        public Task<decimal> SearchVehicleAndCalculateTheAmountPay(int veiculoId, DateTime dataEntrada, DateTime dataSaida);
    }
}
