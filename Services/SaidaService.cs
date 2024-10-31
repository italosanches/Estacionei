using AutoMapper;
using Azure;
using Estacionei.DTOs.Cliente;
using Estacionei.DTOs.Entrada;
using Estacionei.DTOs.Saida;
using Estacionei.Enums;
using Estacionei.Extensions;
using Estacionei.Models;
using Estacionei.Pagination;
using Estacionei.Pagination.Parameters;
using Estacionei.Pagination.Parameters.SaidaParameters;
using Estacionei.Repository;
using Estacionei.Repository.Interfaces;
using Estacionei.Response;
using Estacionei.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Estacionei.Services
{
    public class SaidaService : ISaidaService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public SaidaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseBase<SaidaResponseDto>> CreateSaida(SaidaRequestDto saidaRequestDto)
        {
            var entrada = await _unitOfWork.EntradaRepository.GetAllQueryable().Where(entrada => entrada.EntradaId == saidaRequestDto.EntradaId && entrada.StatusEntrada != StatusEntrada.Fechado)
                                                                               .Include(veiculo => veiculo.Veiculo)
                                                                               .ThenInclude(cliente => cliente.Cliente)
                                                                               .FirstOrDefaultAsync();
            if (entrada == null)
            {
                return ResponseBase<SaidaResponseDto>.FailureResult("Entrada não encontrada, ou entrada esta finalizada.", HttpStatusCode.BadRequest);
            }

            if (saidaRequestDto.DataSaida < entrada.DataEntrada)
            {
                return ResponseBase<SaidaResponseDto>.FailureResult($"Data de saida {saidaRequestDto.DataSaida} esta maior que a data de entrada {entrada.DataEntrada}", HttpStatusCode.BadRequest);
            }
            var saida = _mapper.Map<Saida>(saidaRequestDto);
            saida.ValorCobrado = await _unitOfWork.SaidaRepository.SearchVehicleAndCalculateTheAmountPay(entrada.VeiculoId, entrada.DataEntrada, saidaRequestDto.DataSaida);
            entrada.StatusEntrada = StatusEntrada.Fechado;

            await _unitOfWork.SaidaRepository.AddAsync(saida);
            _unitOfWork.EntradaRepository.UpdateAsync(entrada);

            await _unitOfWork.Commit();
            await _unitOfWork.Dispose();

            var saidaMapped = _mapper.Map<SaidaResponseDto>(saida);

            return ResponseBase<SaidaResponseDto>.SuccessResult(saidaMapped, "Saida criada");


        }

        public async Task<ResponseBase<bool>> DeleteSaida(int saidaId)
        {
            var saidaProcurada = await _unitOfWork.SaidaRepository.GetAsync(saida => saida.SaidaId == saidaId);
            if (saidaProcurada == null)
            {
                return ResponseBase<bool>.FailureResult("Saida não encontrada.", HttpStatusCode.NotFound);
            }
            var entradaProcurada = await _unitOfWork.EntradaRepository.GetAsync(entrada => entrada.EntradaId == saidaProcurada.EntradaId);
            entradaProcurada.StatusEntrada = StatusEntrada.Aberto;
            _unitOfWork.EntradaRepository.UpdateAsync(entradaProcurada);
            _unitOfWork.SaidaRepository.DeleteAsync(saidaProcurada);
            await _unitOfWork.Commit();
            await _unitOfWork.Dispose();
            return ResponseBase<bool>.SuccessResult(true, $"Saida removida e alterado a entrada {entradaProcurada.EntradaId} para em aberto");
        }

        public async Task<ResponseBase<PagedList<SaidaResponseDto>>> GetAllSaidas(SaidaQueryParameters saidaqueryParameters)
        {
            var querySaida = _unitOfWork.SaidaRepository.GetAllQueryable();

            if (saidaqueryParameters.DataInicio != DateTime.MinValue && saidaqueryParameters.DataInicio > saidaqueryParameters.DataFim)
            {
                return ResponseBase<PagedList<SaidaResponseDto>>.FailureResult("Data de início não pode ser maior que a data de fim", HttpStatusCode.BadRequest);
            }
            else
            {   //Filtranda pela data inicio e fim, caso nao seja passado nenhum valor = min value, entao atribui minvalue
                querySaida = querySaida.Where(saida => (saidaqueryParameters.DataInicio == DateTime.MinValue || saida.DataSaida >= saidaqueryParameters.DataInicio) &&
                                                                        (saidaqueryParameters.DataFim == DateTime.MinValue || saida.DataSaida <= saidaqueryParameters.DataFim));
            }
            
            if(saidaqueryParameters.VeiculoId != 0 && saidaqueryParameters.VeiculoPlaca is null)
            {
                querySaida = querySaida.Where(saida => saida.Entrada.VeiculoId == saidaqueryParameters.VeiculoId);
            }
            else if(saidaqueryParameters.VeiculoId <= 0 && saidaqueryParameters.VeiculoPlaca is not null)
            {
                querySaida = querySaida.Where(saida => saida.Entrada.Veiculo.VeiculoPlaca == saidaqueryParameters.VeiculoPlaca.RemoveSpecialCharacters().ToUpper());
            }
            else 
            {
                querySaida = querySaida.Where(saida => saida.Entrada.Veiculo.VeiculoPlaca == saidaqueryParameters.VeiculoPlaca.RemoveSpecialCharacters().ToUpper() &&
                                                       saida.Entrada.VeiculoId == saidaqueryParameters.VeiculoId);
            }
            querySaida = querySaida.AsNoTracking().Include(entrada => entrada.Entrada)
                                                  .ThenInclude(veiculo => veiculo.Veiculo)
                                                  .ThenInclude(cliente => cliente.Cliente);
            var saidasPaginadas = await PagedListService<SaidaResponseDto, Saida>.CreatePagedList(querySaida, saidaqueryParameters, _mapper);
            if (!saidasPaginadas.Any())
            {
                return ResponseBase<PagedList<SaidaResponseDto>>.FailureResult("Não há saidas", HttpStatusCode.NotFound);
            }
            return ResponseBase<PagedList<SaidaResponseDto>>.SuccessResult(saidasPaginadas, "Saidas encontradas.");
        }

        public async Task<ResponseBase<SaidaResponseDto>> GetSaida(int saidaId)
        {
            var saidaProcurada = await _unitOfWork.SaidaRepository.GetAllQueryable()
                                                           .AsNoTracking()
                                                           .Where(saida => saida.SaidaId == saidaId)
                                                           .Include(entrada => entrada.Entrada)
                                                           .ThenInclude(veiculo => veiculo.Veiculo)
                                                           .ThenInclude(cliente => cliente.Cliente)
                                                           .FirstOrDefaultAsync();
            if (saidaProcurada == null)
            {
                return ResponseBase<SaidaResponseDto>.FailureResult("Não há entrada com esse ID", HttpStatusCode.NotFound);
            }
            return ResponseBase<SaidaResponseDto>.SuccessResult(_mapper.Map<SaidaResponseDto>(saidaProcurada), "Saida encontrada");

        }

        public async Task<ResponseBase<SaidaResponseDto>> UpdateDateSaida(SaidaUpdateRequestDto saidaUpdateRequestDto)
        {
            var saidaProcurada = await _unitOfWork.SaidaRepository.GetAllQueryable()
                                                            .Where(saida => saida.SaidaId == saidaUpdateRequestDto.SaidaId)
                                                            .Include(entrada => entrada.Entrada)
                                                            .ThenInclude(veiculo => veiculo.Veiculo).ThenInclude(cliente => cliente.Cliente)
                                                            .FirstOrDefaultAsync();
            if (saidaProcurada is null)
            {
                return ResponseBase<SaidaResponseDto>.FailureResult("Saida não encontrada.", HttpStatusCode.NoContent);
            }

            if (saidaUpdateRequestDto.DataSaida < saidaProcurada.Entrada.DataEntrada)
            {
                return ResponseBase<SaidaResponseDto>.FailureResult($"Data de saida {saidaUpdateRequestDto.DataSaida} esta maior que a data de entrada {saidaProcurada.Entrada.DataEntrada}"
                                                                                    , HttpStatusCode.BadRequest);
            }

            var valorACobrar = await _unitOfWork.SaidaRepository
                                    .SearchVehicleAndCalculateTheAmountPay(saidaProcurada.Entrada.VeiculoId, saidaProcurada.Entrada.DataEntrada, saidaUpdateRequestDto.DataSaida);

            saidaProcurada.ValorCobrado = valorACobrar;
            saidaProcurada.DataSaida = saidaUpdateRequestDto.DataSaida;
            _unitOfWork.SaidaRepository.UpdateAsync(saidaProcurada);
            await _unitOfWork.Commit();
            return ResponseBase<SaidaResponseDto>.SuccessResult(_mapper.Map<SaidaResponseDto>(saidaProcurada), "Saida encontrada");

        }
    }
}
