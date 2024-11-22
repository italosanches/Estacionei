using Estacionei.DTOs.Entrada;
using Estacionei.Pagination;
using Estacionei.Pagination.Parameters.EntradaParameters;
using Estacionei.Response;
namespace Estacionei.Services.Interfaces
{
    public interface IEntryService
    {
        public Task<ResponseBase<PagedList<EntryResponseDto>>> GetAllEntries(EntryQueryParameters queryParameters);
        public Task<ResponseBase<EntryResponseDto>> GetEntryById(int id);
        public Task<ResponseBase<EntryResponseDto>> CreateEntry(EntryRequestDto entryRequestDto);

        public Task<ResponseBase<bool>> UpdateAsync(EntryRequestDto entradaRequestDto);
        public Task<ResponseBase<bool>> DeleteAsync(int id);




    }
}
