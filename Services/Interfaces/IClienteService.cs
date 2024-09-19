using Estacionei.DTOs.Cliente;
using Estacionei.Models;
using Estacionei.Pagination.Parameters;
using Estacionei.Pagination;
using Estacionei.Response;
using Estacionei.Pagination.Parameters.ClienteParameters;

namespace Estacionei.Services.Interfaces
{
    public interface IClienteService
    {
        Task<ResponseBase<IEnumerable<ClienteResponseDto>>> GetAllClienteAsync();
        Task<ResponseBase<PagedList<ClienteResponseDto>>> GetAllClienteByPaginationAsync(ClienteQueryParameters queryParameters);
        Task<ResponseBase<ClienteResponseDto>> GetClienteByIdAsync(int id);
        Task<ResponseBase<ClienteResponseDto>> AddClienteAsync(ClienteRequestCreateDto clienteDto);
        Task<ResponseBase<bool>> UpdateClienteAsync(ClienteRequestUpdateDto clienteDto);
        Task<ResponseBase<bool>> DeleteClienteAsync(int id);
    }
}
