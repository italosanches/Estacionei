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

        public async Task<Veiculo?> GetVeiculoByPlaca(string placa)
        {
           return await _context.Veiculos.AsNoTracking().FirstOrDefaultAsync(x => x.VeiculoPlaca == placa.ToUpper());
        }
    }
}
