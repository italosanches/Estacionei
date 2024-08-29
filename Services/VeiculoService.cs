using AutoMapper;
using Estacionei.DTOs;
using Estacionei.Models;
using Estacionei.Repository.Interfaces;
using Estacionei.Response;
using Estacionei.Services.Interfaces;
using System.Net;

namespace Estacionei.Services
{
    public class VeiculoService : IVeiculoService
    {
        private readonly IVeiculoRepository _veiculoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;

      
        public VeiculoService(IVeiculoRepository veiculoRepository, IClienteRepository clienteRepository,IMapper mapper)
        {
            _veiculoRepository = veiculoRepository;
            _clienteRepository = clienteRepository;
            _mapper            = mapper;
        }

        public async Task<ResponseBase<Veiculo>> AddVeiculoAsync(VeiculoCreateDto veiculoCreateDto)
        {
            var localizarVeiculo = await _veiculoRepository.FindAsync(x => x.VeiculoPlaca.Trim().ToUpper() == veiculoCreateDto.VeiculoPlaca.Trim().ToUpper());
			if (localizarVeiculo.Count() > 0)
			{
				return ResponseBase<Veiculo>.FailureResult("Placa ja existe no banco de dados.", HttpStatusCode.BadRequest);
			}
            var resultCliente = await ClienteExists(veiculoCreateDto.ClienteId);
			if (!resultCliente)
            {
                return ResponseBase<Veiculo>.FailureResult("Cliente não existe.", HttpStatusCode.NotFound);
            };
            
            var veiculo = _mapper.Map<Veiculo>(veiculoCreateDto);
            veiculo.VeiculoPlaca = veiculo.VeiculoPlaca.Trim().ToUpper();
            await _veiculoRepository.AddAsync(veiculo);
            return ResponseBase<Veiculo>.SuccessResult(veiculo, "veiculo cadastrado com sucesso");

        }

        public Task DeleteVeiculoAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Veiculo>> GetAllVeiculoAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Veiculo> GetVeiculoByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Veiculo> GetVeiculoByPlacaAsync(string placa)
        {
            throw new NotImplementedException();
        }

        public Task UpdateVeiculoAsync(Veiculo veiculo)
        {
            throw new NotImplementedException();
        }

        private async Task<bool> ClienteExists(int id)
        {
           var cliente = await _clienteRepository.GetByIdAsync(id);

            return cliente != null;
        }
    }
}
