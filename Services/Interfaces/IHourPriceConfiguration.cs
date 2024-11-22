using Azure;
using Estacionei.DTOs.ConfiguracaoValorHora;
using Estacionei.Enums;
using Estacionei.Models;
using Estacionei.Response;

namespace Estacionei.Services.Interfaces
{
    public interface IHourPriceConfiguration
    {
        Task<ResponseBase<IEnumerable<HourPriceConfigurationResponseDto>>> GetAllConf();
        Task<ResponseBase<HourPriceConfigurationResponseDto>> GetById(int id);

        Task<ResponseBase<HourPriceConfigurationResponseDto>> GetByVehicleType(VehicleType vehicleType);

        Task<ResponseBase<HourPriceConfigurationResponseDto>> CreateConf(HourPriceConfigurationRequestDto hourPriceConfDto);

        Task<ResponseBase<bool>> UpdateConf(HourPriceConfigurationRequestDto hourPriceConfDto);
    }
}
