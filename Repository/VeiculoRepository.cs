using Estacionei.Context;
using Estacionei.Models;
using Estacionei.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Numerics;

namespace Estacionei.Repository
{
	public class VeiculoRepository : IVeiculoRepository
	{
		private readonly AppDbContext _context;

		public VeiculoRepository(AppDbContext context)
		{
			_context = context;
		}
		public async Task<IEnumerable<Veiculo>> GetAllAsync()
		{
			return await _context.Veiculos.AsNoTracking().ToListAsync();

		}
		public async Task<Veiculo> GetByIdAsync(int id)
		{
			return await _context.Veiculos.AsNoTracking().FirstOrDefaultAsync(x => x.VeiculoId == id);

		}

		public async Task<IEnumerable<Veiculo>> FindAsync(Expression<Func<Veiculo, bool>> predicate)
		{
			return await _context.Veiculos.AsNoTracking().Where(predicate).ToListAsync();
		}

		public async Task<int> AddAsync(Veiculo veiculo)
		{

			await _context.Veiculos.AddAsync(veiculo);
			_context.SaveChanges();
			return veiculo.VeiculoId;

		}
		public async Task UpdateAsync(Veiculo veiculo)
		{
			_context.Veiculos.Entry(veiculo).State = EntityState.Modified;
			await _context.SaveChangesAsync();
		}
		public async Task DeleteAsync(Veiculo veiculo)
		{
			_context.Veiculos.Remove(veiculo);
			await _context.SaveChangesAsync();
		}

		
	}
}
