using Estacionei.Models;
using Estacionei.Repository.Interfaces;
using Estacionei.Response;
using Estacionei.Services.Interfaces;

namespace Estacionei.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<ResponseBase<Cliente>> AddClienteAsync(Cliente cliente)
        {
          await _clienteRepository.AddAsync(cliente);
           return ResponseBase<Cliente>.SuccessResult(cliente, "cliente cadastrado com sucesso");
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

        public async Task<Cliente> GetClienteByIdAsync(int id)
        {
          return await _clienteRepository.GetByIdAsync(id);
        }

        public Task<Cliente> GetClienteByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

       

    }
}
