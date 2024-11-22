using Estacionei.Models;

namespace Estacionei.Repository.Interfaces
{
    public interface IEntryRepository : IRepository<Entry>
    {
        public Task<bool> HasOpenEntryForVehicle(int idVehicle);
    }
    
}
