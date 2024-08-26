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
				await _context.SaveChangesAsync();
				return cliente.ClienteId;
			}
			catch (DbUpdateException dbEx) 
			{
				throw new Exception($"Erro ao salvar o cliente no banco: {dbEx.InnerException?.Message}");
			}
			catch (Exception ex)
			{
				throw new Exception($"Erro ao salvar o cliente: {ex.Message}");
			}

		}

		public Task DeleteAsync(int id)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<Cliente>> GetAllAsync()
		{
			return await _context.Clientes.AsNoTracking().ToListAsync();
		}

		public async Task<Cliente> GetByIdAsync(int id)
		{
			try
			{
				return await _context.Clientes.AsNoTracking().FirstOrDefaultAsync(x => x.ClienteId == id);
			}
			catch (Exception ex)
			{
				throw new Exception($"Erro ao pesquisar o cliente no banco {ex.Message}");
			}
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
