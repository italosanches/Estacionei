using AutoMapper;
using Estacionei.DTOs.Cliente;
using Estacionei.Mapping;
using Estacionei.Models;
using Estacionei.Repository.Interfaces;
using Estacionei.Response;
using Estacionei.Services.Interfaces;
using System.Net;
using System.Reflection.Metadata;

namespace Estacionei.Services
{
    public class ClienteService : IClienteService
	{
		private readonly IClienteRepository _clienteRepository;
		private readonly IMapper _mapper;

		public ClienteService(IClienteRepository clienteRepository, IMapper mapper)
		{	
			_clienteRepository = clienteRepository;
			_mapper = mapper;
		}

		public async Task<ResponseBase<ClienteGetDto>> AddClienteAsync(ClienteCreateDto clienteDto)
		{
            var cliente = _mapper.Map<Cliente>(clienteDto);
            await _clienteRepository.AddAsync(cliente);
            return ResponseBase<ClienteGetDto>.SuccessResult(_mapper.Map<ClienteGetDto>(cliente), "Cliente cadastrado com sucesso.");
        }

		public async Task<ResponseBase<bool>> UpdateClienteAsync(int id,ClienteUpdateDto clienteDto)
		{
			var cliente = await GetCliente(id);
			if(cliente == null)
			{
                return ResponseBase<bool>.FailureResult("Cliente atualizado com sucesso.",HttpStatusCode.NotFound);
            }
            await _clienteRepository.UpdateAsync(_mapper.Map<Cliente>(clienteDto));
			return ResponseBase<bool>.SuccessResult(true,"Cliente atualizado com sucesso.");
		}
		public async Task<ResponseBase<bool>> DeleteClienteAsync(int id)
		{
			var cliente = await GetCliente(id);
			if(cliente == null)
			{
				return ResponseBase<bool>.FailureResult("Cliente não encontrado.", HttpStatusCode.NotFound);
			}
			await _clienteRepository.DeleteAsync(cliente);
            return ResponseBase<bool>.SuccessResult(true, "Cliente deletado com sucesso.");


        }

        public async Task<ResponseBase<IEnumerable<ClienteGetDto>>> GetAllClienteAsync()
		{
            var clientes = await _clienteRepository.GetAllAsync();
            var clientesDto = _mapper.Map<IEnumerable<ClienteGetDto>>(clientes);
            return ResponseBase<IEnumerable<ClienteGetDto>>.SuccessResult(clientesDto, "Lista de clientes");
        }

		public async Task<ResponseBase<ClienteGetDto>> GetClienteByIdAsync(int id)
		{
            var cliente = await _clienteRepository.GetByIdAsync(id);
            if (cliente == null)
            {
                return ResponseBase<ClienteGetDto>.FailureResult("Cliente não encontrado",HttpStatusCode.NotFound);
            }

            return ResponseBase<ClienteGetDto>.SuccessResult(_mapper.Map<ClienteGetDto>(cliente), "Cliente encontrado");
        }

		private async Task<Cliente> GetCliente(int id)
		{
			var cliente = await _clienteRepository.GetByIdAsync(id);
			return cliente;
		}
	}
}

