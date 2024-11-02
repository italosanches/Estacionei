using Azure;
using Estacionei.DTOs.ConfiguracaoValorHora;
using Estacionei.Enums;
using Estacionei.Models;
using Estacionei.Response;

namespace Estacionei.Services.Interfaces
{
    public interface IConfiguracaoValorHoraService
    {
        Task<ResponseBase<IEnumerable<ConfiguracaoValorHoraResponseDto>>> GetAllConf();
        Task<ResponseBase<ConfiguracaoValorHoraResponseDto>> GetById(int id);

        Task<ResponseBase<ConfiguracaoValorHoraResponseDto>> GetByTipoVeiculo(TipoVeiculo tipoVeiculo);

        Task<ResponseBase<ConfiguracaoValorHoraResponseDto>> CreateConf(ConfiguracaoValorHoraRequestDto confValorHoraCreateDto);

        Task<ResponseBase<bool>> UpdateConf(ConfiguracaoValorHoraRequestDto confValorHoraUpdateDto);
    }
}
