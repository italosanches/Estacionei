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

		public async Task<bool> DeleteAsync(Cliente cliente)
		{
			try
			{
				_context.Clientes.Remove(cliente);
				await _context.SaveChangesAsync();
				return true;
			}
			catch (Exception ex) 
			{ 
				throw new Exception($"Erro ao remover o clientes no banco:  {ex.InnerException?.Message}");
			}
		}

		public async Task<IEnumerable<Cliente>> GetAllAsync()
		{
			try
			{
				return await _context.Clientes.AsNoTracking().ToListAsync();
			}
			catch (Exception ex) 
			{ 
				throw new Exception($"Erro ao pesquisar os clientes no banco:  {ex.InnerException?.Message}");
			}

		}

		public async Task<Cliente> GetByIdAsync(int id)
		{
			try
			{
				return await _context.Clientes.AsNoTracking().FirstOrDefaultAsync(x => x.ClienteId == id);
			}
			catch (Exception ex)
			{
				throw new Exception($"Erro ao pesquisar o cliente no banco:  {ex.InnerException?.Message}");
			}
		}

		public async Task<bool> UpdateAsync(Cliente cliente)
		{
			try
			{
				_context.Entry(cliente).State = EntityState.Modified;
				await _context.SaveChangesAsync();
				return true;
			}
			catch(DbUpdateException dbEx)
			{
				throw new Exception($"Erro ao atualizar o cliente no banco: {dbEx.InnerException?.Message}");

			}
			catch (Exception ex)
			{
				throw new Exception($"Erro ao atualizar o cliente no banco: {ex.InnerException?.Message}");
			}
		}
	}
}
