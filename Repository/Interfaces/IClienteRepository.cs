using Estacionei.Models;

namespace Estacionei.Repository.Interfaces
{
    public interface IClienteRepository
    {
        Task<IEnumerable<Cliente>> GetAllAsync();
        Task<Cliente> GetByIdAsync(int id);

        Task<Cliente> GetByNameAsync(string name);
        Task<int> AddAsync(Cliente cliente);
        Task UpdateAsync(Cliente cliente);
        Task DeleteAsync(int id);


    }
}
