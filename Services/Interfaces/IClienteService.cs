using Estacionei.DTOs.Cliente;
using Estacionei.Models;
using Estacionei.Response;

namespace Estacionei.Services.Interfaces
{
    public interface IClienteService
    {
        Task<ResponseBase<IEnumerable<ClienteResponseDto>>> GetAllClienteAsync();
        Task<ResponseBase<ClienteResponseDto>> GetClienteByIdAsync(int id);
        Task<ResponseBase<ClienteResponseDto>> AddClienteAsync(ClienteRequestCreateDto clienteDto);
        Task<ResponseBase<bool>> UpdateClienteAsync(ClienteRequestUpdateDto clienteDto);
        Task<ResponseBase<bool>> DeleteClienteAsync(int id);
    }
}
