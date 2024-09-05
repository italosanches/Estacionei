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

        public async Task<IEnumerable<Cliente>> GetAllClienteAndVeiculos()
        {
            return await _context.Clientes.AsNoTracking().Include(x => x.VeiculosCliente).ToListAsync();
        }
        public async Task<Cliente?> GetClienteAndVeiculos(int id)
        {
            return await _context.Clientes.AsNoTracking().Include(x => x.VeiculosCliente).FirstOrDefaultAsync(x => x.ClienteId == id);
        }
    }
}
