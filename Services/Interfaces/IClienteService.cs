using Estacionei.Models;
using Estacionei.Response;

namespace Estacionei.Services.Interfaces
{
    public interface IClienteService
    {
        Task<IEnumerable<Cliente>> GetAllClienteAsync();
        Task<Cliente> GetClienteByIdAsync(int id);
        Task<Cliente> GetClienteByNameAsync(string name);
        Task<ResponseBase<Cliente>> AddClienteAsync(Cliente cliente);
        Task<ResponseBase<Cliente>> UpdateClienteAsync(Cliente cliente);
        Task<ResponseBase<Cliente>> DeleteClienteAsync(int id);
    }
}
