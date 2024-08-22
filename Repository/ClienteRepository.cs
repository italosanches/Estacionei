using Estacionei.Context;
using Estacionei.Models;
using Estacionei.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Estacionei.Repository
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly AppDbContext _context;

        public ClienteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Cliente cliente)
        {
            try
            {
                await _context.Clientes.AddAsync(cliente);
                _context.SaveChanges();
                return cliente.ClienteId;
            }
            catch (Exception ex)
            {

                throw new Exception($"Erro ao salvar o cliente no banco {ex.Message}");
            }
          
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Cliente>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Cliente> GetByIdAsync(int id)
        {
           return await _context.Clientes.AsNoTracking().FirstOrDefaultAsync(x=> x.ClienteId == id);
        }

        public Task<Cliente> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Cliente cliente)
        {
            throw new NotImplementedException();
        }
    }
}
