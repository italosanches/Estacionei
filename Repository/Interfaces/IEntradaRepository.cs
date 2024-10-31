using Estacionei.Models;

namespace Estacionei.Repository.Interfaces
{
    public interface IEntradaRepository : IRepository<Entrada>
    {
        public Task<bool> HasOpenEntryForVehicle(int idVeiculo);
    }
    
}
