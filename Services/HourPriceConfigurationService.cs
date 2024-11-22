using AutoMapper;
using Azure;
using Estacionei.DTOs.ConfiguracaoValorHora;
using Estacionei.Enums;
using Estacionei.Models;
using Estacionei.Repository.Interfaces;
using Estacionei.Response;
using Estacionei.Services.Interfaces;
using System.Collections.Generic;
using System.Net;

namespace Estacionei.Services
{
    public class HourPriceConfigurationService : IHourPriceConfiguration
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HourPriceConfigurationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseBase<HourPriceConfigurationResponseDto>> CreateConf(HourPriceConfigurationRequestDto hourPriceConfDto)
        {
            var checkVehicleType = await _unitOfWork.HourPriceConfigurationRepository.GetAsync(x => x.VehicleType == hourPriceConfDto.VehicleType);
            if (checkVehicleType != null)
            {
                return ResponseBase<HourPriceConfigurationResponseDto>.FailureResult("Configuração para esse tipo de veiculo ja existe no banco de dados.", HttpStatusCode.BadRequest);
            }
            var hourPriceConf = _mapper.Map<HourPriceConfiguration>(hourPriceConfDto);
            await _unitOfWork.HourPriceConfigurationRepository.AddAsync(hourPriceConf);
            await _unitOfWork.Commit();
            await _unitOfWork.Dispose();
            return ResponseBase<HourPriceConfigurationResponseDto>.SuccessResult(_mapper.Map<HourPriceConfigurationResponseDto>(hourPriceConf), "Configuração criada");

        }

        public async Task<ResponseBase<IEnumerable<HourPriceConfigurationResponseDto>>> GetAllConf()
        {
            var hourPriceConfs = await _unitOfWork.HourPriceConfigurationRepository.GetAllAsync();
            var hourPriceConfsDto = _mapper.Map<IEnumerable<HourPriceConfigurationResponseDto>>(hourPriceConfs);
            if(!hourPriceConfsDto.Any())
            {
                return ResponseBase<IEnumerable<HourPriceConfigurationResponseDto>>.FailureResult("Não há registros no banco.", HttpStatusCode.NotFound);

            }
            return ResponseBase<IEnumerable<HourPriceConfigurationResponseDto>>.SuccessResult(hourPriceConfsDto, "Configurações encontradas.");
        }

        public async Task<ResponseBase<HourPriceConfigurationResponseDto>> GetById(int id)
        {
            var hourPriceConf = await _unitOfWork.HourPriceConfigurationRepository.GetAsync(conf => conf.HourPriceConfigurationId == id);
            if (hourPriceConf is null)
            {
                return ResponseBase<HourPriceConfigurationResponseDto>.FailureResult("Não foi encontrado.", HttpStatusCode.NotFound);
            }
            return ResponseBase<HourPriceConfigurationResponseDto>.SuccessResult(_mapper.Map<HourPriceConfigurationResponseDto>(hourPriceConf), "Configuração encontrada.");
        }

        public async Task<ResponseBase<HourPriceConfigurationResponseDto>> GetByVehicleType(VehicleType vehicleType)
        {
            var hourPriceConf = await _unitOfWork.HourPriceConfigurationRepository.GetAsync(conf => conf.VehicleType == vehicleType);
            if (hourPriceConf is null)
            {
                return ResponseBase<HourPriceConfigurationResponseDto>.FailureResult("Não há configuração para esse tipo de veiculo.", HttpStatusCode.NotFound);
            }
            return ResponseBase<HourPriceConfigurationResponseDto>.SuccessResult(_mapper.Map<HourPriceConfigurationResponseDto>(hourPriceConf), "Configuração encontrada.");
        }

        public async Task<ResponseBase<bool>> UpdateConf(HourPriceConfigurationRequestDto hourPriceConfDto)
        {
            var hourPriceConf = await _unitOfWork.HourPriceConfigurationRepository.GetAsync(conf => conf.VehicleType == hourPriceConfDto.VehicleType);

            if(hourPriceConf is null)
            {
                return ResponseBase<bool>.FailureResult("Não há configurações criadas para esse tipo de veiculo.", HttpStatusCode.NotFound);
            }
            if(hourPriceConf.HourPriceConfigurationId != hourPriceConfDto.HourPriceConfigurationId)
            {
                return ResponseBase<bool>.FailureResult("Ids divergentes da configuração existente para esse tipo de veiculo.", HttpStatusCode.Conflict);

            }
            _unitOfWork.HourPriceConfigurationRepository.UpdateAsync(_mapper.Map<HourPriceConfiguration>(hourPriceConfDto));
            await _unitOfWork.Commit();
            await _unitOfWork.Dispose();
            return ResponseBase<bool>.SuccessResult(true, "Configuração atualizada.");
        }
    }
}
