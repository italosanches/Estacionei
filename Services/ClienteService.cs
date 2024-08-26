using AutoMapper;
using Estacionei.DTOs;
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
		public Task<ResponseBase<Cliente>> UpdateClienteAsync(Cliente cliente)
		{
			throw new NotImplementedException();
		}
		public Task<ResponseBase<Cliente>> DeleteClienteAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Cliente>> GetAllClienteAsync()
		{
			throw new NotImplementedException();
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

		public Task<ResponseBase<ClienteGetDto>> GetClienteByNameAsync(string name)
		{
			throw new NotImplementedException();
		}
	}
}

