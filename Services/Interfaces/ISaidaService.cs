using Estacionei.DTOs.Saida;
using Estacionei.Pagination;
using Estacionei.Pagination.Parameters.SaidaParameters;
using Estacionei.Response;

namespace Estacionei.Services.Interfaces
{
    public interface ISaidaService
    {
        public Task<ResponseBase<SaidaResponseDto>> CreateSaida(SaidaRequestDto saidaRequestDto);
        public Task<ResponseBase<PagedList<SaidaResponseDto>>> GetAllSaidas(SaidaQueryParameters saidaqueryParameters);
        public Task<ResponseBase<SaidaResponseDto>> GetSaida(int saidaId);

        public Task<ResponseBase<SaidaResponseDto>> UpdateDateSaida(SaidaUpdateRequestDto saidaUpdateRequestDto);
        public Task<ResponseBase<bool>> DeleteSaida (int saidaId);
    }
}
