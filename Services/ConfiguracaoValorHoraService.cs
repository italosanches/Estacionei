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
    public class ConfiguracaoValorHoraService : IConfiguracaoValorHoraService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ConfiguracaoValorHoraService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseBase<ConfiguracaoValorHoraGetDto>> CreateConf(ConfiguracaoValorHoraCreateDto confValorHoraCreateDto)
        {
            var verificarTipoVeiculo = await _unitOfWork.ConfiguracaoValorHoraRepository.GetAsync(x => x.TipoVeiculo == confValorHoraCreateDto.TipoVeiculo);
            if (verificarTipoVeiculo != null)
            {
                return ResponseBase<ConfiguracaoValorHoraGetDto>.FailureResult("Configuração para esse tipo de veiculo ja existe no banco de dados.", HttpStatusCode.BadRequest);
            }
            var configuracaoValorHora = _mapper.Map<ConfiguracaoValorHora>(confValorHoraCreateDto);
            await _unitOfWork.ConfiguracaoValorHoraRepository.AddAsync(configuracaoValorHora);
            await _unitOfWork.Commit();
            await _unitOfWork.Dispose();
            return ResponseBase<ConfiguracaoValorHoraGetDto>.SuccessResult(_mapper.Map<ConfiguracaoValorHoraGetDto>(configuracaoValorHora), "Configuração criada");

        }

        public async Task<ResponseBase<IEnumerable<ConfiguracaoValorHoraGetDto>>> GetAllConf()
        {
            var confs = await _unitOfWork.ConfiguracaoValorHoraRepository.GetAllAsync();
            var confsDto = _mapper.Map<IEnumerable<ConfiguracaoValorHoraGetDto>>(confs);
            return ResponseBase<IEnumerable<ConfiguracaoValorHoraGetDto>>.SuccessResult(confsDto ?? new List<ConfiguracaoValorHoraGetDto>(), "Configurações encontradas.");
        }

        public async Task<ResponseBase<ConfiguracaoValorHoraGetDto>> GetById(int id)
        {
            var result = await _unitOfWork.ConfiguracaoValorHoraRepository.GetAsync(conf => conf.Id == id);
            if (result is null)
            {
                return ResponseBase<ConfiguracaoValorHoraGetDto>.FailureResult("Não foi encontrado.", HttpStatusCode.NotFound);
            }
            return ResponseBase<ConfiguracaoValorHoraGetDto>.SuccessResult(_mapper.Map<ConfiguracaoValorHoraGetDto>(result), "Configuração encontrada.");
        }

        public async Task<ResponseBase<ConfiguracaoValorHoraGetDto>> GetByTipoVeiculo(TipoVeiculo tipoVeiculo)
        {
            var result = await _unitOfWork.ConfiguracaoValorHoraRepository.GetAsync(conf => conf.TipoVeiculo == tipoVeiculo);
            if (result is null)
            {
                return ResponseBase<ConfiguracaoValorHoraGetDto>.FailureResult("Não há configuração para esse tipo de veiculo.", HttpStatusCode.NotFound);
            }
            return ResponseBase<ConfiguracaoValorHoraGetDto>.SuccessResult(_mapper.Map<ConfiguracaoValorHoraGetDto>(result), "Configuração encontrada.");
        }

        public async Task<ResponseBase<bool>> UpdateConf(ConfiguracaoValorHoraUpdateDto confValorHoraUpdateDto)
        {
            var result = await _unitOfWork.ConfiguracaoValorHoraRepository.GetAsync(conf => conf.TipoVeiculo == confValorHoraUpdateDto.TipoVeiculo);

            if(result is null)
            {
                return ResponseBase<bool>.FailureResult("Não há configurações criadas para esse tipo de veiculo.", HttpStatusCode.NotFound);
            }
            if(result.Id !=  confValorHoraUpdateDto.Id)
            {
                return ResponseBase<bool>.FailureResult("Ids divergentes da configuração existente para esse tipo de veiculo.", HttpStatusCode.BadRequest);

            }
            await _unitOfWork.ConfiguracaoValorHoraRepository.UpdateAsync(_mapper.Map<ConfiguracaoValorHora>(confValorHoraUpdateDto));
            await _unitOfWork.Commit();
            await _unitOfWork.Dispose();
            return ResponseBase<bool>.SuccessResult(true, "Configuração atualizada.");
        }
    }
}
