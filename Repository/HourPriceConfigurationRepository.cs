using Estacionei.Context;
using Estacionei.Models;
using Estacionei.Repository.Interfaces;

namespace Estacionei.Repository
{
    public class HourPriceConfigurationRepository : Repository<HourPriceConfiguration> , IHourPriceConfigurationRepository
    {
        public HourPriceConfigurationRepository(AppDbContext context) : base(context)
        {

        }
    }
}
