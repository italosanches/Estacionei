using Estacionei.DTOs.Entrada;
using Estacionei.Pagination;
using Estacionei.Pagination.Parameters.EntradaParameters;
using Estacionei.Response;
namespace Estacionei.Services.Interfaces
{
    public interface IEntradaService
    {
        public Task<ResponseBase<PagedList<EntradaResponseDto>>> GetAllEntradas(EntradaQueryParameters queryParameters);
        public Task<ResponseBase<EntradaResponseDto>> GetEntradaById(int id);
        public Task<ResponseBase<EntradaResponseDto>> CreateEntrada(EntradaRequestDto entradaRequestDto);

        public Task<ResponseBase<bool>> Update(EntradaRequestDto entradaRequestDto);
        public Task<ResponseBase<bool>> Delete(int id);




    }
}
