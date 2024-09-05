using Estacionei.Context;
using Estacionei.Models;
using Estacionei.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Estacionei.Repository
{
	public class ClienteRepository : Repository<Cliente> , IClienteRepository
	{

        public ClienteRepository(AppDbContext context) : base(context)
		{
			
		}

	//	public async Task<IEnumerable<Cliente>> GetAllAsync()
	//	{
 //           return await _context.Clientes.AsNoTracking().Include(x => x.VeiculosCliente).ToListAsync();
 //       }

 //       public async Task<Cliente> GetByIdAsync(int id)
	//	{
 //           return await _context.Clientes.AsNoTracking().Include(x =>x.VeiculosCliente).FirstOrDefaultAsync(x => x.ClienteId == id);
 //       }
 //       //Expression é para usar lambda, a func recebe um cliente e retorna os clientes onde
 //       //a condicao seja true
	//	public async Task<IEnumerable<Cliente>> FindAsync(Expression<Func<Cliente, bool>> predicate)
	//	{
	//		return await _context.Clientes.AsNoTracking().Where(predicate).ToListAsync();
	//	}
	//	public async Task<int> AddAsync(Cliente cliente)
 //       {
 //           await _context.Clientes.AddAsync(cliente);
 //           await _context.SaveChangesAsync();
 //           return cliente.ClienteId;

 //       }
 //       public async Task DeleteAsync(Cliente cliente)
 //       {
 //           _context.Clientes.Remove(cliente);
 //           await _context.SaveChangesAsync();
 //       }
 //       public async Task UpdateAsync(Cliente cliente)
	//	{
 //           _context.Clientes.Entry(cliente).State = EntityState.Modified;
 //           await _context.SaveChangesAsync();
 //       }

		
	}
}
