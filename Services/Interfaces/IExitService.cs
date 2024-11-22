using Estacionei.DTOs.Saida;
using Estacionei.Pagination;
using Estacionei.Pagination.Parameters.SaidaParameters;
using Estacionei.Response;

namespace Estacionei.Services.Interfaces
{
    public interface IExitService
    {
        public Task<ResponseBase<ExitResponseDto>> CreateExit(ExitRequestDto exitRequestDto);
        public Task<ResponseBase<PagedList<ExitResponseDto>>> GetAllExits(ExitQueryParameters exitqueryParameters);
        public Task<ResponseBase<ExitResponseDto>> GetExitById(int exitId);

        public Task<ResponseBase<ExitResponseDto>> UpdateExitDate(ExitUpdateRequestDto exitUpdateRequestDto);
        public Task<ResponseBase<bool>> DeleteExit (int exitId);
    }
}
