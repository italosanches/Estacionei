using AutoMapper;
using Estacionei.DTOs;
using Estacionei.DTOs.Cliente;
using Estacionei.Extensions;
using Estacionei.Mapping;
using Estacionei.Models;
using Estacionei.Pagination;
using Estacionei.Pagination.Parameters;
using Estacionei.Pagination.Parameters.ClienteParameters;
using Estacionei.Repository.Interfaces;
using Estacionei.Response;
using Estacionei.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;

namespace Estacionei.Services
{
    public class ClienteService : IClienteService
    {
       
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ClienteService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseBase<PagedList<ClienteResponseDto>>> GetAllClienteByPaginationAsync(ClienteQueryParameters clienteQueryParameters)
        {
            var clientesQueryable = _unitOfWork.ClienteRepository.GetAllQueryable().AsNoTracking();

            if (clienteQueryParameters.ClienteNome is not null)
            {
                clientesQueryable = clientesQueryable.Where(cliente => cliente.ClienteNome.ToUpper().Contains(clienteQueryParameters.ClienteNome.ToUpper().RemoveSpecialCharacters()));
            }

            clientesQueryable = clientesQueryable.Include(cli => cli.VeiculosCliente);
            // Obtém a lista paginada
            var clientesPaginados = await PagedListService<ClienteResponseDto, Cliente>.CreatePagedList(clientesQueryable, clienteQueryParameters, _mapper);

            if (!clientesPaginados.Any())
            {
                return ResponseBase<PagedList<ClienteResponseDto>>.FailureResult("Não há registros no banco.", HttpStatusCode.NotFound);
            }    

            return ResponseBase<PagedList<ClienteResponseDto>>.SuccessResult(clientesPaginados, "Clientes paginados");
        }


        public async Task<ResponseBase<ClienteResponseDto>> GetClienteByIdAsync(int id)
        {
            var cliente = await _unitOfWork.ClienteRepository.GetClienteAndVeiculos(id); 
            if (cliente == null)
            {
                return ResponseBase<ClienteResponseDto>.FailureResult("Cliente não encontrado", HttpStatusCode.NotFound);
            }

            return ResponseBase<ClienteResponseDto>.SuccessResult(_mapper.Map<ClienteResponseDto>(cliente), "Cliente encontrado");
        }

        public async Task<ResponseBase<ClienteResponseDto>> AddClienteAsync(ClienteRequestCreateDto clienteDto)
        {
            var cliente = _mapper.Map<Cliente>(clienteDto);

            if (clienteDto.Veiculo == null)
            {
                await _unitOfWork.ClienteRepository.AddAsync(cliente);
                await _unitOfWork.Commit();
                await _unitOfWork.Dispose();


                return ResponseBase<ClienteResponseDto>.SuccessResult(_mapper.Map<ClienteResponseDto>(cliente), "Cliente cadastrado com sucesso.");
            }
            else
            {
                var veiculo = _mapper.Map<Veiculo>(clienteDto.Veiculo);
                var veiculoExist = await _unitOfWork.VeiculoRepository.GetVeiculoByPlacaAsync(veiculo.VeiculoPlaca.Replace(" ", "").ToUpper().RemoveSpecialCharacters());
                if (veiculoExist != null)
                {
                    return ResponseBase<ClienteResponseDto>.FailureResult("Placa ja existe no banco de dados.", HttpStatusCode.BadRequest);
                }
                await _unitOfWork.ClienteRepository.AddAsync(cliente);
                await _unitOfWork.Commit();
                veiculo.ClienteId = cliente.ClienteId;
                veiculo.VeiculoPlaca = veiculo.VeiculoPlaca.RemoveSpecialCharacters().Replace(" ", "").ToUpper();

                await _unitOfWork.VeiculoRepository.AddAsync(veiculo);
                await _unitOfWork.Commit();
                await _unitOfWork.Dispose();
                return ResponseBase<ClienteResponseDto>.SuccessResult(_mapper.Map<ClienteResponseDto>(cliente), "Cliente cadastrado com sucesso.");

            }
        }

        public async Task<ResponseBase<bool>> UpdateClienteAsync(ClienteRequestUpdateDto clienteDto)
        {
            var cliente = await _unitOfWork.ClienteRepository.GetClienteAndVeiculos(clienteDto.ClienteId);
            if (cliente == null)
            {
                return ResponseBase<bool>.FailureResult("Cliente não encontrado.", HttpStatusCode.NotFound);
            }
             _unitOfWork.ClienteRepository.UpdateAsync(_mapper.Map<Cliente>(clienteDto));
            await _unitOfWork.Commit();
            await _unitOfWork.Dispose();
            return ResponseBase<bool>.SuccessResult(true, "Cliente atualizado com sucesso.");
        }
        public async Task<ResponseBase<bool>> DeleteClienteAsync(int id)
        {
            var cliente = await _unitOfWork.ClienteRepository.GetClienteAndVeiculos(id); 
            if (cliente == null)
            {
                return ResponseBase<bool>.FailureResult("Cliente não encontrado.", HttpStatusCode.NotFound);
            }
             _unitOfWork.ClienteRepository.DeleteAsync(cliente);
            await _unitOfWork.Commit();
            await _unitOfWork.Dispose();
            return ResponseBase<bool>.SuccessResult(true, "Cliente deletado com sucesso.");


        }
    }
}

