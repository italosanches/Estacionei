using Estacionei.Context;
using Estacionei.Models;
using Estacionei.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Numerics;

namespace Estacionei.Repository
{
	public class VeiculoRepository : Repository<Veiculo> , IVeiculoRepository
	{
        public VeiculoRepository(AppDbContext context) : base(context)
        {
            
        }
        //private readonly AppDbContext _context;

        //public VeiculoRepository(AppDbContext context)
        //{
        //	_context = context;
        //}
        //public async Task<IEnumerable<Veiculo>> GetAllAsync()
        //{
        //	return await _context.Veiculos.AsNoTracking().ToListAsync();

        //}
        //public async Task<Veiculo?> GetAsync(Expression<Func<Veiculo, bool>> predicate)
        //      {
        //	return await _context.Veiculos.AsNoTracking().FirstOrDefaultAsync(predicate);

        //}

        //public async Task<IEnumerable<Veiculo>> FindAsync(Expression<Func<Veiculo, bool>> predicate)
        //{
        //	return await _context.Veiculos.AsNoTracking().Where(predicate).ToListAsync();
        //}

        //public async Task<Veiculo> AddAsync(Veiculo veiculo)
        //{

        //	await _context.Veiculos.AddAsync(veiculo);
        //	_context.SaveChanges();
        //	return veiculo;

        //}
        //public async Task<Veiculo> UpdateAsync(Veiculo veiculo)
        //{
        //	_context.Veiculos.Entry(veiculo).State = EntityState.Modified;
        //	await _context.SaveChangesAsync();
        //	return veiculo;
        //}
        //public async Task<Veiculo> DeleteAsync(Veiculo veiculo)
        //{
        //	_context.Veiculos.Remove(veiculo);
        //	await _context.SaveChangesAsync();
        //	return veiculo;
        //}


    }
}
