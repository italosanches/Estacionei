using AutoMapper;
using Estacionei.DTOs.Entrada;
using Estacionei.Enums;
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

        public async Task<ResponseBase<EntradaResponseDto>> CreateEntrada(EntradaRequestCreateDto entradaRequestCreateDto)
        {
            var veiculo = await GetVeiculo(entradaRequestCreateDto.VeiculoId);
            if (veiculo is null)
            {
                return ResponseBase<EntradaResponseDto>.FailureResult("Veiculo não encontrado.", HttpStatusCode.NotFound);
            }
            if (await VerificarEntradaEmAbertoVeiculo(entradaRequestCreateDto.VeiculoId))
            {
                return ResponseBase<EntradaResponseDto>.FailureResult("Ja existe uma entrada em aberto para esse veiculo.", HttpStatusCode.BadRequest);
            }
            if (entradaRequestCreateDto.DataEntrada <= DateTime.UtcNow.AddHours(-1))
            {
                return ResponseBase<EntradaResponseDto>.FailureResult("Entrada deve ser maior que a data de hoje", HttpStatusCode.BadRequest);

            }
            var novaEntrada = _mapper.Map<Entrada>(entradaRequestCreateDto);
            await _unitOfWork.EntradaRepository.AddAsync(novaEntrada);
            await _unitOfWork.Commit();
            await _unitOfWork.Dispose();
            return ResponseBase<EntradaResponseDto>.SuccessResult(_mapper.Map<EntradaResponseDto>(novaEntrada), "Entrada criada");

        }

        public async Task<ResponseBase<PagedList<EntradaResponseDto>>> GetAllEntradas(EntradaQueryParameters queryParameters)
        {
            var entradasQueryable = _unitOfWork.EntradaRepository.GetAllQueryable().Include(x=> x.Veiculo).Include(x=>x.Veiculo.Cliente);

            if (queryParameters.DataInicio != DateTime.MinValue)
            {
                entradasQueryable.Where(entrada => entrada.DataEntrada >= queryParameters.DataInicio);
            }
            entradasQueryable.Where(entrada => entrada.DataEntrada <= queryParameters.DataFim);



            var entradaPagedList = await PagedListService<EntradaResponseDto, Entrada>.CreatePagedList(entradasQueryable, queryParameters, _mapper);
            if (entradaPagedList.Count() <= 0)
            {

                return ResponseBase<PagedList<EntradaResponseDto>>.FailureResult("Não há entradas cadastradas", HttpStatusCode.NotFound);
            }
            var entradasDto = _mapper.Map<IEnumerable<EntradaResponseDto>>(entradasQueryable);
            return ResponseBase<PagedList<EntradaResponseDto>>.SuccessResult(entradaPagedList, "Entradas encontradas");

        }

        public async Task<ResponseBase<EntradaResponseDto>> GetEntradaById(int id)
        {
            var entradas = await _unitOfWork.EntradaRepository.GetAsync(entrada => entrada.EntradaId == id);
            if (entradas == null)
            {

                return ResponseBase<EntradaResponseDto>.FailureResult("Não há entrada com esse id", HttpStatusCode.NotFound);
            }
            var entradasDto = _mapper.Map<EntradaResponseDto>(entradas);
            return ResponseBase<EntradaResponseDto>.SuccessResult(entradasDto, "Entradas encontradas");
        }

        public async Task<ResponseBase<IEnumerable<EntradaResponseDto>>> GetEntradaByVeiculoId(int veiculoId)
        {
            var veiculo = await GetVeiculo(veiculoId);
            if (veiculo is null)
            {
                return ResponseBase<IEnumerable<EntradaResponseDto>>.FailureResult("Veiculo não encontrado.", HttpStatusCode.NotFound);

            }
            var entradas = await _unitOfWork.EntradaRepository.GetAsync(entrada => entrada.VeiculoId == veiculoId);
            if (entradas == null)
            {

                return ResponseBase<IEnumerable<EntradaResponseDto>>.FailureResult("Não há entrada com esse id", HttpStatusCode.NotFound);
            }
            var entradasDto = _mapper.Map<IEnumerable<EntradaResponseDto>>(entradas);
            return ResponseBase<IEnumerable<EntradaResponseDto>>.SuccessResult(entradasDto, "Entradas encontradas");
        }

        private async Task<Veiculo> GetVeiculo(int id)
        {
            return await _unitOfWork.VeiculoRepository.GetAsync(x => x.VeiculoId == id);
        }

        private async Task<bool> VerificarEntradaEmAbertoVeiculo(int idVeiculo)
        {
            var result = await _unitOfWork.EntradaRepository.FindAsync(entrada => entrada.VeiculoId == idVeiculo && entrada.StatusEntrada == StatusEntrada.Aberto);
            return result.Count() > 0;
        }
    }
}
