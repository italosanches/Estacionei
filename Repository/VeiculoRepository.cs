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

        public async Task<Veiculo?> GetVeiculoByPlacaAsync(string placa)
        {
           return await _context.Veiculos.AsNoTracking().FirstOrDefaultAsync(x => x.VeiculoPlaca == placa);
        }

        public async Task<IEnumerable<Veiculo?>> GetVeiculoByClienteAsync(int id)
        {
            return await _context.Veiculos.AsNoTracking().Where(x=> x.ClienteId == id).ToListAsync();
        }
    }
}
