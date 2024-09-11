using AutoMapper;
using Estacionei.DTOs;
using Estacionei.DTOs.Cliente;
using Estacionei.Extensions;
using Estacionei.Mapping;
using Estacionei.Models;
using Estacionei.Repository;
using Estacionei.Repository.Interfaces;
using Estacionei.Response;
using Estacionei.Services.Interfaces;
using System.Net;
using System.Reflection.Metadata;

namespace Estacionei.Services
{
    public class ClienteService : IClienteService
    {
        //private readonly IClienteRepository _clienteRepository;
        ////private readonly IRepository<Cliente> _repository;
        //private readonly IVeiculoRepository _unitOfWork.VeiculoRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ClienteService(IMapper mapper,IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseBase<IEnumerable<ClienteGetDto>>> GetAllClienteAsync()
        {
            var clientes = await _unitOfWork.ClienteRepository.GetAllClienteAndVeiculos();
            var clientesDto = _mapper.Map<IEnumerable<ClienteGetDto>>(clientes);
            return ResponseBase<IEnumerable<ClienteGetDto>>.SuccessResult(clientesDto, "Lista de clientes");
        }

        public async Task<ResponseBase<ClienteGetDto>> GetClienteByIdAsync(int id)
        {
            var cliente = await GetCliente(id);
            if (cliente == null)
            {
                return ResponseBase<ClienteGetDto>.FailureResult("Cliente não encontrado", HttpStatusCode.NotFound);
            }

            return ResponseBase<ClienteGetDto>.SuccessResult(_mapper.Map<ClienteGetDto>(cliente), "Cliente encontrado");
        }

        public async Task<ResponseBase<ClienteGetDto>> AddClienteAsync(ClienteCreateDto clienteDto)
        {
            var cliente = _mapper.Map<Cliente>(clienteDto);

            if (clienteDto.Veiculo == null)
            {
                await _unitOfWork.ClienteRepository.AddAsync(cliente);
                await _unitOfWork.Commit();
                await _unitOfWork.Dispose();


                return ResponseBase<ClienteGetDto>.SuccessResult(_mapper.Map<ClienteGetDto>(cliente), "Cliente cadastrado com sucesso.");
            }
            else
            {
                var veiculo = _mapper.Map<Veiculo>(clienteDto.Veiculo);
                var veiculoExist = await _unitOfWork.VeiculoRepository.GetVeiculoByPlaca(veiculo.VeiculoPlaca.Replace(" ", "").ToUpper().RemoveSpecialCharacters());
                if (veiculoExist != null)
                {
                    return ResponseBase<ClienteGetDto>.FailureResult("Placa ja existe no banco de dados.", HttpStatusCode.BadRequest);
                }
                await _unitOfWork.ClienteRepository.AddAsync(cliente);
                await _unitOfWork.Commit();
                veiculo.ClienteId = cliente.ClienteId;
                veiculo.VeiculoPlaca = veiculo.VeiculoPlaca.RemoveSpecialCharacters().Replace(" ", "").ToUpper();

                await _unitOfWork.VeiculoRepository.AddAsync(veiculo);
                await _unitOfWork.Commit();
                await _unitOfWork.Dispose();
                return ResponseBase<ClienteGetDto>.SuccessResult(_mapper.Map<ClienteGetDto>(cliente), "Cliente cadastrado com sucesso.");

            }
        }

        public async Task<ResponseBase<bool>> UpdateClienteAsync(ClienteUpdateDto clienteDto)
        {
            var cliente = await GetCliente(clienteDto.ClienteId);
            if (cliente == null)
            {
                return ResponseBase<bool>.FailureResult("Cliente não encontrado.", HttpStatusCode.NotFound);
            }
            await _unitOfWork.ClienteRepository.UpdateAsync(_mapper.Map<Cliente>(clienteDto));
            await _unitOfWork.Commit();
            await _unitOfWork.Dispose();
            return ResponseBase<bool>.SuccessResult(true, "Cliente atualizado com sucesso.");
        }
        public async Task<ResponseBase<bool>> DeleteClienteAsync(int id)
        {
            var cliente = await GetCliente(id);
            if (cliente == null)
            {
                return ResponseBase<bool>.FailureResult("Cliente não encontrado.", HttpStatusCode.NotFound);
            }
            await _unitOfWork.ClienteRepository.DeleteAsync(cliente);
            await _unitOfWork.Commit();
            await _unitOfWork.Dispose();
            return ResponseBase<bool>.SuccessResult(true, "Cliente deletado com sucesso.");


        }


        private async Task<Cliente> GetCliente(int id)
        {
            var cliente = await _unitOfWork.ClienteRepository.GetClienteAndVeiculos(id);
            return cliente;
        }
    }
}

