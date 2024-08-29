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

	

		public async Task<IEnumerable<Cliente>> GetAllAsync()
		{
            return await _context.Clientes.AsNoTracking().ToListAsync();
        }

        public async Task<Cliente> GetByIdAsync(int id)
		{
            return await _context.Clientes.AsNoTracking().FirstOrDefaultAsync(x => x.ClienteId == id);
        }
        public async Task<int> AddAsync(Cliente cliente)
        {
            await _context.Clientes.AddAsync(cliente);
            await _context.SaveChangesAsync();
            return cliente.ClienteId;

        }
        public async Task<bool> DeleteAsync(Cliente cliente)
        {
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(Cliente cliente)
		{
            _context.Entry(cliente).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }
	}
}
