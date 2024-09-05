
using Estacionei.Models;

namespace Estacionei.Repository.Interfaces
{
    public interface IClienteRepository : IRepository<Cliente>
    {
       Task<IEnumerable<Cliente>> GetAllClienteAndVeiculos();
       Task<Cliente?> GetClienteAndVeiculos(int id);
    }
}
