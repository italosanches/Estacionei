using Azure;
using Estacionei.DTOs.ConfiguracaoValorHora;
using Estacionei.Enums;
using Estacionei.Models;
using Estacionei.Response;

namespace Estacionei.Services.Interfaces
{
    public interface IConfiguracaoValorHoraService
    {
        Task<ResponseBase<IEnumerable<ConfiguracaoValorHoraGetDto>>> GetAllConf();
        Task<ResponseBase<ConfiguracaoValorHoraGetDto>> GetById(int id);

        Task<ResponseBase<ConfiguracaoValorHoraGetDto>> GetByTipoVeiculo(TipoVeiculo tipoVeiculo);

        Task<ResponseBase<ConfiguracaoValorHoraGetDto>> CreateConf(ConfiguracaoValorHoraCreateDto confValorHoraCreateDto);

        Task<ResponseBase<bool>> UpdateConf(ConfiguracaoValorHoraUpdateDto confValorHoraUpdateDto);
    }
}
