using Estacionei.Context;
using Estacionei.Enums;
using Estacionei.Models;
using Estacionei.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Estacionei.Repository
{
    public class EntradaRepository : Repository<Entrada> , IEntradaRepository
    {
        public EntradaRepository(AppDbContext context) :base(context)
        {
            
        }
        public async Task<bool> HasOpenEntryForVehicle(int idVeiculo)
        {
            return await _context.Entrada.AsNoTracking().AnyAsync(entrada => entrada.VeiculoId == idVeiculo && entrada.StatusEntrada == StatusEntrada.Aberto);
            
        }
    }
}

