using AutoMapper;
using Estacionei.DTOs.Cliente;
using Estacionei.Mapping;
using Estacionei.Models;
using Estacionei.Repository.Interfaces;
using Estacionei.Response;
using Estacionei.Services.Interfaces;
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
			try
			{
				var cliente = _mapper.Map<Cliente>(clienteDto);
				await _clienteRepository.AddAsync(cliente);
				return ResponseBase<ClienteGetDto>.SuccessResult(_mapper.Map<ClienteGetDto>(cliente), "Cliente cadastrado com sucesso");
			}
			catch (Exception ex)
			{
				return ResponseBase<ClienteGetDto>.FailureResult($"Erro ao salvar o cliente {ex.Message}");

			}
		}

		public async Task<ResponseBase<ClienteUpdateDto>> UpdateClienteAsync(int id, ClienteUpdateDto clienteDto)
		{
			try
			{
				if (id != clienteDto.ClienteId)
				{
					return ResponseBase<ClienteUpdateDto>.FailureResult("Ids divergentes");
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public Task<ResponseBase<Cliente>> DeleteClienteAsync(int id)
		{
			throw new NotImplementedException();
		}

		public async Task<ResponseBase<IEnumerable<ClienteGetDto>>> GetAllClienteAsync()
		{
			try
			{
				var clientes = await _clienteRepository.GetAllAsync();
				var clientesDto = _mapper.Map<IEnumerable<ClienteGetDto>>(clientes);
				return ResponseBase<IEnumerable<ClienteGetDto>>.SuccessResult(clientesDto,"Lista de clientes");
			}
			catch (Exception ex)
			{
				return ResponseBase<IEnumerable<ClienteGetDto>>.FailureResult($"Erro ao pesquisar cliente {ex.Message}");
			}
		}

		public async Task<ResponseBase<ClienteGetDto>> GetClienteByIdAsync(int id)
		{
			try
			{
				var cliente = await _clienteRepository.GetByIdAsync(id);
				if (cliente == null)
				{
					return ResponseBase<ClienteGetDto>.FailureResult("Cliente não encontrado");
				}

				return ResponseBase<ClienteGetDto>.SuccessResult(_mapper.Map<ClienteGetDto>(cliente), "Cliente encontrado");
			}
			catch (Exception ex)
			{
				return ResponseBase<ClienteGetDto>.FailureResult($"Erro ao pesquisar cliente {ex.Message}");
			}
		}
	}
}

