using Estacionei.DTOs;
using Estacionei.Models;
using Estacionei.Response;

namespace Estacionei.Services.Interfaces
{
    public interface IClienteService
    {
        Task<IEnumerable<Cliente>> GetAllClienteAsync();
        Task<ResponseBase<ClienteGetDto>> GetClienteByIdAsync(int id);
        Task<ResponseBase<ClienteGetDto>> GetClienteByNameAsync(string name);
        Task<ResponseBase<ClienteGetDto>> AddClienteAsync(ClienteCreateDto clienteDto);
        Task<ResponseBase<Cliente>> UpdateClienteAsync(Cliente cliente);
        Task<ResponseBase<Cliente>> DeleteClienteAsync(int id);
    }
}
