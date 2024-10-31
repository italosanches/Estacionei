using AutoMapper;
using Estacionei.DTOs.Entrada;
using Estacionei.Enums;
using Estacionei.Extensions;
using Estacionei.Models;
using Estacionei.Pagination;
using Estacionei.Pagination.Parameters.EntradaParameters;
using Estacionei.Repository.Interfaces;
using Estacionei.Response;
using Estacionei.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Estacionei.Services
{
    public class EntradaService : IEntradaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EntradaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResponseBase<PagedList<EntradaResponseDto>>> GetAllEntradas(EntradaQueryParameters queryParameters)
        {
            var entradasQueryable = _unitOfWork.EntradaRepository.GetAllQueryable();

            if (queryParameters.DataFim != DateTime.MinValue && queryParameters.DataInicio > queryParameters.DataFim)
            {
                return ResponseBase<PagedList<EntradaResponseDto>>.FailureResult("Data de início não pode ser maior que a data de fim", HttpStatusCode.BadRequest);
            }
            else
            {   //Filtranda pela data inicio e fim, caso nao seja passado nenhum valor = min value, entao o filtro nao é aplicado
                entradasQueryable = entradasQueryable.Where(entrada => (queryParameters.DataInicio == DateTime.MinValue || entrada.DataEntrada >= queryParameters.DataInicio) &&
                                                                       (queryParameters.DataFim == DateTime.MinValue || entrada.DataEntrada <= queryParameters.DataFim));
            }
            if (queryParameters.VeiculoId != 0 && queryParameters.VeiculoPlaca is null)
            {
                entradasQueryable = entradasQueryable.Where(entrada => entrada.VeiculoId == queryParameters.VeiculoId);
            }
            else if(queryParameters.VeiculoId <= 0 && queryParameters.VeiculoPlaca is not null)
            {
                entradasQueryable = entradasQueryable.Where(entrada => entrada.Veiculo.VeiculoPlaca == queryParameters.VeiculoPlaca.RemoveSpecialCharacters().ToUpper());
            }
            else 
            {
                entradasQueryable = entradasQueryable.Where(entrada => entrada.Veiculo.VeiculoPlaca == queryParameters.VeiculoPlaca.RemoveSpecialCharacters().ToUpper() &&
                                                                       entrada.VeiculoId == queryParameters.VeiculoId);
            }
            entradasQueryable = entradasQueryable.Include(x => x.Veiculo).ThenInclude(v => v.Cliente);
            var entradaPagedList = await PagedListService<EntradaResponseDto, Entrada>.CreatePagedList(entradasQueryable, queryParameters, _mapper);
            if (!entradaPagedList.Any())
            {
                return ResponseBase<PagedList<EntradaResponseDto>>.FailureResult("Não há entradas cadastradas", HttpStatusCode.NotFound);
            }
            var entradasDto = _mapper.Map<IEnumerable<EntradaResponseDto>>(entradasQueryable);
            return ResponseBase<PagedList<EntradaResponseDto>>.SuccessResult(entradaPagedList, "Entradas encontradas");

        }

        public async Task<ResponseBase<EntradaResponseDto>> GetEntradaById(int id)
        {
            var entrada = await _unitOfWork.EntradaRepository.GetAllQueryable().Include(entrada => entrada.Veiculo)
                                                                                   .ThenInclude(veiculo => veiculo.Cliente)
                                                                                   .Where(entrada => entrada.EntradaId == id).FirstOrDefaultAsync();
            if (entrada == null)
            {

                return ResponseBase<EntradaResponseDto>.FailureResult("Não há entrada com esse id", HttpStatusCode.NotFound);
            }
            var entradasDto = _mapper.Map<EntradaResponseDto>(entrada);
            return ResponseBase<EntradaResponseDto>.SuccessResult(entradasDto, "Entradas encontradas");
        }

        public async Task<ResponseBase<EntradaResponseDto>> CreateEntrada(EntradaRequestDto entradaRequestDto)
        {
            var veiculo = await _unitOfWork.VeiculoRepository.GetAllQueryable().Where(veiculoProcurado => veiculoProcurado.VeiculoId == entradaRequestDto.VeiculoId)
                                                                               .Include(x => x.Cliente).FirstOrDefaultAsync();
            if (veiculo is null)
            {
                return ResponseBase<EntradaResponseDto>.FailureResult("Veiculo não encontrado.", HttpStatusCode.NotFound);
            }
            if (await _unitOfWork.EntradaRepository.HasOpenEntryForVehicle(veiculo.VeiculoId))
            {
                return ResponseBase<EntradaResponseDto>.FailureResult("Ja existe uma entrada em aberto para esse veiculo.", HttpStatusCode.BadRequest);
            }
            if (entradaRequestDto.DataEntrada > DateTime.UtcNow)
            {
                return ResponseBase<EntradaResponseDto>.FailureResult("Entrada deve ser menor que a data de hoje", HttpStatusCode.BadRequest);

            }
            var novaEntrada = _mapper.Map<Entrada>(entradaRequestDto);
            await _unitOfWork.EntradaRepository.AddAsync(novaEntrada);
            await _unitOfWork.Commit();
            await _unitOfWork.Dispose();
            novaEntrada.Veiculo = veiculo;
            var entradaResponse = _mapper.Map<EntradaResponseDto>(novaEntrada);
            return ResponseBase<EntradaResponseDto>.SuccessResult(entradaResponse, "Entrada criada");


        }

        public async Task<ResponseBase<bool>> Update(EntradaRequestDto entradaRequestDto)
        {
            var entradaProcurada = await _unitOfWork.EntradaRepository.GetAsync(entrada => entrada.EntradaId == entradaRequestDto.EntradaId);
            if (entradaProcurada == null)
            {
                return ResponseBase<bool>.FailureResult("Entrada não encotrada.", HttpStatusCode.NotFound);

            }
            var saidaProcurada = await _unitOfWork.SaidaRepository.GetAsync(entrada => entrada.EntradaId == entradaProcurada.EntradaId);
            if (saidaProcurada != null)
            {
                return ResponseBase<bool>.FailureResult($"Existe uma saida vinculada.Id: {saidaProcurada.SaidaId}, remova antes de alterar a entrada.", HttpStatusCode.Conflict);
            }
            if (await _unitOfWork.VeiculoRepository.GetAsync(veiculo => veiculo.VeiculoId == entradaRequestDto.VeiculoId) == null)
            {
                return ResponseBase<bool>.FailureResult("Veiculo não encontrado.", HttpStatusCode.NotFound);
            }
            if (entradaRequestDto.DataEntrada > DateTime.UtcNow)
            {
                 return ResponseBase<bool>.FailureResult("Entrada deve ser menor que a data de hoje", HttpStatusCode.BadRequest);
            }
            if(entradaRequestDto.VeiculoId != entradaProcurada.VeiculoId)
            {
                var VerificarSeVeiculoTemEntradaEmAberto =await _unitOfWork.EntradaRepository
                                                          .GetAsync(entrada => entrada.VeiculoId == entradaRequestDto.VeiculoId && entrada.StatusEntrada == StatusEntrada.Aberto);
                if(VerificarSeVeiculoTemEntradaEmAberto != null)
                {
                    return ResponseBase<bool>.FailureResult($"Existe uma entrada em aberto para o veiculo {VerificarSeVeiculoTemEntradaEmAberto.VeiculoId}", HttpStatusCode.Conflict);
                }
            }

            _unitOfWork.EntradaRepository.UpdateAsync(_mapper.Map<Entrada>(entradaRequestDto));
            await _unitOfWork.Commit();
            await _unitOfWork.Dispose();
            return ResponseBase<bool>.SuccessResult(true, "Entrada atualizada");
        }

        public async Task<ResponseBase<bool>> Delete(int id)
        {
            var entradaProcurada =await _unitOfWork.EntradaRepository.GetAsync(entrada => entrada.EntradaId == id);
            if (entradaProcurada == null)
            {
                return ResponseBase<bool>.FailureResult("Entrada não encotrada.", HttpStatusCode.NotFound);

            }
            var saidaProcurada = await _unitOfWork.SaidaRepository.GetAsync(entrada => entrada.EntradaId == entradaProcurada.EntradaId);
            if (saidaProcurada != null)
            {
                return ResponseBase<bool>.FailureResult($"Existe uma saida vinculada.Id: {saidaProcurada.SaidaId}, remova antes de deletar a entrada.", HttpStatusCode.Conflict);
            }
             _unitOfWork.EntradaRepository.DeleteAsync(_mapper.Map<Entrada>(entradaProcurada));
            await _unitOfWork.Commit();
            return ResponseBase<bool>.SuccessResult(true, "Entrada removida.");
        }
    }
}
