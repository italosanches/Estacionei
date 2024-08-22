using Estacionei.Models;
using Estacionei.Repository.Interfaces;
using Estacionei.Response;
using Estacionei.Services.Interfaces;

namespace Estacionei.Services
{
    public class VeiculoService : IVeiculoService
    {
        private readonly IVeiculoRepository _veiculoRepository;
        private readonly IClienteRepository _clienteRepository;

      
        public VeiculoService(IVeiculoRepository veiculoRepository, IClienteRepository clienteRepository)
        {
            _veiculoRepository = veiculoRepository;
            _clienteRepository = clienteRepository;
        }

        public async Task<ResponseBase<Veiculo>> AddVeiculoAsync(Veiculo veiculo)
        {
            //IEnumerable<Veiculo> veiculoList = await _veiculoRepository.GetAllAsync();
            var localizarVeiculo = await _veiculoRepository.GetByPlacaAsync(veiculo.VeiculoPlaca) ?? null;
            if(!await ClienteExists(veiculo.ClienteId)) 
            {
                return ResponseBase<Veiculo>.FailureResult("Cliente não existe.");
            };
            if (localizarVeiculo != null)
            {
                return ResponseBase<Veiculo>.FailureResult("Placa ja existe no banco de dados.");
            }
            veiculo.VeiculoPlaca = veiculo.VeiculoPlaca.Trim().ToUpper();
            await _veiculoRepository.AddAsync(veiculo);
            return ResponseBase<Veiculo>.SuccessResult(veiculo,"veiculo cadastrado com sucesso");

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
