using Estacionei.Context;
using Estacionei.Models;
using Estacionei.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Numerics;

namespace Estacionei.Repository
{
	public class VehicleRepository : Repository<Vehicle> , IVehicleRepository
	{
        public VehicleRepository(AppDbContext context) : base(context)
        {
            
        }

      
        public async Task<Vehicle?> GetVehicleByLicensePlateAsync(string licensePlate)
        {
           return await _context.Vehicles.AsNoTracking().FirstOrDefaultAsync(x => x.VehicleLicensePlate == licensePlate);
        }

        
    }
}
