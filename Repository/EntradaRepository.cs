using Estacionei.Context;
using Estacionei.Models;
using Estacionei.Repository.Interfaces;

namespace Estacionei.Repository
{
    public class EntradaRepository : Repository<Entrada> , IEntradaRepository
    {
        public EntradaRepository(AppDbContext context) :base(context)
        {
            
        }
    }
}
