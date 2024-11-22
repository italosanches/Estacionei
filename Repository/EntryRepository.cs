using Estacionei.Context;
using Estacionei.Enums;
using Estacionei.Models;
using Estacionei.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Estacionei.Repository
{
    public class EntryRepository : Repository<Entry> , IEntryRepository
    {
        public EntryRepository(AppDbContext context) :base(context)
        {
            
        }
        public async Task<bool> HasOpenEntryForVehicle(int idVeiculo)
        {
            return await _context.Entries.AsNoTracking().AnyAsync(entrada => entrada.VehicleId == idVeiculo && entrada.EntryStatus == EntryStatus.Open);
            
        }
    }
}

