using Estacionei.Context;
using Estacionei.Enums;
using Estacionei.Models;
using Estacionei.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Estacionei.Repository
{
    public class ExitRepository : Repository<Exit>, IExitRepository
    {
        public ExitRepository(AppDbContext context) : base(context)
        {
        }
        
        public async Task<decimal> SearchVehicleAndCalculateTheAmountPay(int vehicleId, DateTime entryDate, DateTime exitDate)
        {
            var searchedVehicleType = (await _context.Vehicles
                                              .AsNoTracking()
                                              .Where(searchedVehicle => searchedVehicle.VehicleId == vehicleId)
                                              .FirstOrDefaultAsync())?.VehicleType;

            var amountToCharge = (await _context.HourPriceConfigurations
                               .AsNoTracking()
                               .Where(hourPriceConfiguration => hourPriceConfiguration.VehicleType == searchedVehicleType)
                               .FirstOrDefaultAsync())?.HourlyRate;

            if (amountToCharge == null)
            {
               throw new NullReferenceException(message: "Não há configuração de valor hora, cadastre antes de continuar.");
            }

            TimeSpan hours = exitDate - entryDate;
            double totalHours = Math.Round(hours.TotalHours, MidpointRounding.AwayFromZero);

            return Math.Round(amountToCharge * (decimal)totalHours ?? 0, 2);
        }
    }
}
