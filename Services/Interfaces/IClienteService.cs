using Estacionei.DTOs;
using Estacionei.Models;
using Estacionei.Response;

namespace Estacionei.Services.Interfaces
{
    public interface IClienteService
    {
        Task<IEnumerable<Cliente>> GetAllClienteAsync();
        Task<ResponseBase<Cliente>> GetClienteByIdAsync(int id);
        Task<Cliente> GetClienteByNameAsync(string name);
        Task<ResponseBase<Cliente>> AddClienteAsync(ClienteDTO clienteDto);
        Task<ResponseBase<Cliente>> UpdateClienteAsync(Cliente cliente);
        Task<ResponseBase<Cliente>> DeleteClienteAsync(int id);
    }
}
